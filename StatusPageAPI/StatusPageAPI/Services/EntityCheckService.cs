using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArgonautCore.Network.Http;
using Microsoft.Extensions.Logging;
using StatusPageAPI.Models;
using StatusPageAPI.Models.Enums;

namespace StatusPageAPI.Services
{
    public class EntityCheckService
    {
        private readonly ILogger<EntityCheckService> _log;
        private readonly CoreHttpClient _http;
        private readonly EntityConfigService _ecs;
        private readonly StatusService _ss;

        private const int _REFRESH_CD_MIN = 5;

        // ReSharper disable once NotAccessedField.Local
        private readonly Timer _timer;

        public EntityCheckService(ILogger<EntityCheckService> log, CoreHttpClient http, EntityConfigService ecs, StatusService ss)
        {
            _log = log;
            _http = http;
            _ecs = ecs;
            _ss = ss;

            // After startup it should do it's first query after x seconds. This gives enough time for everything to warm up and start
            _timer = new Timer(this.GetStatuses, null, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(_REFRESH_CD_MIN));
            
            _log.LogInformation("Initialized Entity Check service");
        }

        private async void GetStatuses(object state)
        {
            _log.LogInformation("Getting Statuses...");
            var entities = _ecs.GetEntityDeclarations();
            var statuses = new List<EntityStatus>(entities.Count);
            foreach (var entity in entities)
            {
                if (entity.IsCategory)
                {
                    var s = await this.GetEntityWithSubEntities(entity);
                    statuses.Add(s);
                }
                else
                {
                    var res = await _http.GetAndMapResponse<EntityStatus>(entity.HealthEndpoint.ToString());
                    if (!res)
                    {
                        statuses.Add(new EntityStatus()
                        {
                            Identifier = entity.Identifier,
                            Description = entity.Description,
                            Error = res.Err().Message,
                            Status = Status.Outage
                        });
                        continue;
                    }

                    var s = (~res).SetOverrides(entity);
                    statuses.Add(s);
                }
            }
            _ss.SetStatuses(statuses);
            _log.LogInformation("Finished updating all the status information.");
        }

        private async Task<EntityStatus> GetEntityWithSubEntities(EntityDeclaration e)
        {
            var ent = new EntityStatus()
            {
                Identifier = e.Identifier,
                Description = e.Description,
                SubEntities = new List<EntityStatus>()
            };
            foreach (var entity in e.SubEntities)
            {
                var res = await _http.GetAndMapResponse<EntityStatus>(entity.HealthEndpoint.ToString());
                if (!res)
                {
                    ent.SubEntities.Add(new EntityStatus()
                    {
                        Identifier = entity.Identifier,
                        Description = entity.Description,
                        Error = res.Err().Message,
                        Status = Status.Outage
                    });
                    continue;
                }

                var s = (~res).SetOverrides(entity);
                ent.SubEntities.Add(s);
            }

            // TODO this hurts my ETH heart. Maybe wanna optimise this even tho its not rly needed since its a rly small list
            if (ent.SubEntities.Any(x => x.Status == Status.PartialOutage || x.Status == Status.Outage))
                ent.Status = Status.PartialOutage;
            if (ent.SubEntities.All(x => x.Status == Status.Outage))
                ent.Status = Status.Outage;

            return ent;
        }
    }
}