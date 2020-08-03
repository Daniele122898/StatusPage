using ArgonautCore.Network.Http;
using Microsoft.Extensions.Logging;

namespace StatusPageAPI.Services
{
    public class EntityCheckService
    {
        private readonly ILogger<EntityCheckService> _log;
        private readonly CoreHttpClient _http;

        public EntityCheckService(ILogger<EntityCheckService> log, CoreHttpClient http)
        {
            _log = log;
            _http = http;
            
            _log.LogInformation("Initialized Entity Check service");
        }
    }
}