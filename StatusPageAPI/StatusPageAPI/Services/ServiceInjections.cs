using ArgonautCore.Network.Http;
using Microsoft.Extensions.DependencyInjection;

namespace StatusPageAPI.Services
{
    public static class ServiceInjections
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddSingleton<EntityCheckService>()
                .AddSingleton<CoreHttpClient>()
                .AddSingleton<StatusService>();
    }
}