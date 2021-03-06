﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArgonautCore.Lw;
using ArgonautCore.Network.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StatusPageAPI.Models;
using StatusPageAPI.Models.Configurations;
using StatusPageAPI.Models.Enums;

namespace StatusPageAPI.Services
{
    public class EntityCheckService
    {
        private readonly ILogger<EntityCheckService> _log;
        private readonly CoreHttpClient _http;
        private readonly EntityConfigService _ecs;
        private readonly StatusService _ss;

        private readonly int _refreshCdSecs;

        // ReSharper disable once NotAccessedField.Local
        private readonly Timer _timer;

        public EntityCheckService(ILogger<EntityCheckService> log, CoreHttpClient http, 
            EntityConfigService ecs, StatusService ss, IOptions<AuthenticationConfig> authConfig)
        {
            _log = log;
            _http = http;
            _ecs = ecs;
            _ss = ss;

            _refreshCdSecs = authConfig.Value.EntityCooldownSeconds;

            // After startup it should do it's first query after x seconds. This gives enough time for everything to warm up and start
            _timer = new Timer(this.GetStatuses, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(_refreshCdSecs));

            _log.LogInformation("Initialized Entity Check service");
        }

        private async void GetStatuses(object state)
        {
            try
            {
                _log.LogInformation("Getting Statuses...");
                _timer.Change(int.MaxValue, int.MaxValue);
                var entities = await _ecs.GetEntityDeclarationsAsync();
                var statuses = new List<EntityStatus>(entities.Count);
                foreach (var entity in entities)
                {
                    if (entity.IsCategory)
                    {
                        if (entity.SubEntities == null || entity.SubEntities.Count == 0)
                            continue;

                        var s = await this.GetEntityWithSubEntities(entity);
                        statuses.Add(s);
                    }
                    else
                    {
                        var sw = new Stopwatch();
                        sw.Restart();
                        Result<EntityStatus, Error> res = default;
                        try
                        {
                            res = await _http.GetAndMapResponse<EntityStatus>(entity.HealthEndpoint.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        uint rtt = (uint) sw.ElapsedMilliseconds;
                        sw.Stop();
                        if (res == default || !res)
                        {
                            statuses.Add(new EntityStatus()
                            {
                                Identifier = entity.Identifier,
                                Description = entity.Description,
                                Error = "Could not reach service. It is most likely completely down and unreachable.",
                                Status = Status.Outage
                            });
                            continue;
                        }

                        var s = (~res).SetOverrides(entity);
                        s.RTT = rtt;
                        statuses.Add(s);
                    }
                }

                _ss.SetStatuses(statuses);
                _log.LogInformation("Finished updating all the status information.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _timer.Change(TimeSpan.FromSeconds(_refreshCdSecs), TimeSpan.FromSeconds(_refreshCdSecs));
            }
        }

        private async Task<EntityStatus> GetEntityWithSubEntities(EntityDeclaration e)
        {
            var ent = new EntityStatus()
            {
                Identifier = e.Identifier,
                Description = e.Description,
                SubEntities = new List<EntityStatus>()
            };
            var sw = new Stopwatch();
            foreach (var entity in e.SubEntities)
            {
                sw.Restart();
                Result<EntityStatus, Error> res = default;
                try
                {
                    res = await _http.GetAndMapResponse<EntityStatus>(entity.HealthEndpoint.ToString());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                uint rtt = (uint) sw.ElapsedMilliseconds;
                if (res == default || !res)
                {
                    ent.SubEntities.Add(new EntityStatus()
                    {
                        Identifier = entity.Identifier,
                        Description = entity.Description,
                        Error = "Could not reach service. It is most likely completely down and unreachable.",
                        Status = Status.Outage
                    });
                    continue;
                }

                var s = (~res).SetOverrides(entity);
                s.RTT = rtt;
                ent.SubEntities.Add(s);
            }
            
            // TODO this hurts my ETH heart. Maybe wanna optimise this even tho its not rly needed since its a rly small list
            if (ent.SubEntities.Any(x => x.Status == Status.PartialOutage || x.Status == Status.Outage))
                ent.Status = Status.PartialOutage;
            if (ent.SubEntities.All(x => x.Status == Status.Outage))
                ent.Status = Status.Outage;

            // Get the average RTT on the category
            ent.RTT = (uint) ent.SubEntities.Where(x => x.Status != Status.Outage).Average(x => x.RTT);

            return ent;
        }
    }
}