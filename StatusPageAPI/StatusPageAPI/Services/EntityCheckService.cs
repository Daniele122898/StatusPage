using System;
using System.Collections.Generic;
using System.Threading;
using ArgonautCore.Network.Enums;
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
        
        private const int _REFRESH_CD_MIN = 5;

        // ReSharper disable once NotAccessedField.Local
        private readonly Timer _timer;

        public EntityCheckService(ILogger<EntityCheckService> log, CoreHttpClient http, EntityConfigService ecs)
        {
            _log = log;
            _http = http;
            _ecs = ecs;

            // After startup it should do it's first query after x seconds. This gives enough time for everything to warm up and start
            _timer = new Timer(this.GetStatuses, null, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(_REFRESH_CD_MIN));
            
            _log.LogInformation("Initialized Entity Check service");
        }

        private async void GetStatuses(object state)
        {
            var entities = _ecs.GetEntityDeclarations();
            int n = entities.Count;
            var statuses = new List<EntityStatus>(n);
            for (int i = 0; i < n; i++)
            {
                var e = entities[i];
                var res = await _http.GetAndMapResponse<EntityStatus>(e.HealthEndpoint.ToString());
                if (!res)
                {
                    statuses.Add(new EntityStatus()
                    {
                        Identifier = e.Identifier,
                        Error = res.Err().Message,
                        Status = Status.Outage
                    });
                }
            }
        }
    }
}