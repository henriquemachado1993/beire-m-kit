using BeireMKit.Cache.Configuration;
using BeireMKit.Cache.Interfaces;
using BeireMKit.Cache.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeireMKit.Cache.Extensions
{
    public static class ServiceCollections
    {
        public static IServiceCollection ConfigureMemoryCache(this IServiceCollection services, IConfiguration configuration)
        {
            _ = SetCacheConfiguration(services, configuration);

            services.AddDistributedMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }

        public static IServiceCollection ConfigureRedisCache(this IServiceCollection services, IConfiguration configuration, string? instanceName = null)
        {
            var cacheConfiguration = SetCacheConfiguration(services, configuration);

            if(string.IsNullOrEmpty(cacheConfiguration.UrlConnection))
                throw new ArgumentException(nameof(cacheConfiguration.UrlConnection));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheConfiguration.UrlConnection;
                options.InstanceName = instanceName;
            });
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }

        private static ICacheConfiguration SetCacheConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfiguration = CacheConfiguration.Bind(configuration);
            services.AddSingleton(cacheConfiguration);

            return cacheConfiguration;
        }
    }
}
