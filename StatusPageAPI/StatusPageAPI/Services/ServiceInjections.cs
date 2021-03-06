﻿using System;
using ArgonautCore.Network.Http;
using Microsoft.Extensions.DependencyInjection;

namespace StatusPageAPI.Services
{
    public static class ServiceInjections
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
            => services
                .AddSingleton<EntityCheckService>()
                .AddSingleton(new CoreHttpClient(TimeSpan.FromSeconds(2)))
                .AddSingleton<StatusService>()
                .AddSingleton<EntityConfigService>()
                .AddSingleton<SpecialNoticeService>();
    }
}