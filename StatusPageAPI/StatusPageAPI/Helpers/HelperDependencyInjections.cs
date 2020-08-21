using Microsoft.Extensions.DependencyInjection;

namespace StatusPageAPI.Helpers
{
    public static class HelperDependencyInjections
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
            => services;
    }
}