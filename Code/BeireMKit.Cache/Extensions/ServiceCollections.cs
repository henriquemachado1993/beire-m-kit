using BeireMKit.Cache.Services;
using BeireMKit.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BeireMKit.Cache.Extensions
{
    public static class ServiceCollections
    {
        public static IServiceCollection ConfigureMemoryCache(this IServiceCollection services)
        {
            services.AddScoped<ICacheService, MemoryCacheService>();
            return services;
        }

        public static IServiceCollection ConfigureRedisCache(this IServiceCollection services, string connection, string? instanceName = null)
        {
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connection;
                options.InstanceName = instanceName; // Opcional
            });
            return services;
        }
    }
}
