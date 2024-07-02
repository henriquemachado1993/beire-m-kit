using BeireMKit.Infra.Interfaces.ApiService;
using BeireMKit.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BeireMKit.Infra.Extensions
{
    public static class ServiceCollections
    {
        public static IServiceCollection ConfigureApiService(this IServiceCollection services)
        {
            services.AddScoped<IApiServiceFactory, ApiServiceFactory>();
            return services;
        }
    }
}
