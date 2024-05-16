using BeireMKit.Notification.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BeireMKit.Notification.Extensions
{
    public static class ServiceCollections
    {
        /// <summary>
        /// Configuring notification related services
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureNotification(this IServiceCollection services)
        {
            services.AddScoped<INotification, Notification>();
            return services;
        }

        /// <summary>
        /// Configuring notification context related services
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureNotificationContext(this IServiceCollection services)
        {
            services.AddSingleton<NotificationContext>();
            return services;
        }
    }
}
