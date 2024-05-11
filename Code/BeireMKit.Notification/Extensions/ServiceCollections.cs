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
        public static void ConfigureNotification(this IServiceCollection services)
        {
            //services.AddScoped(typeof(INotification), provider => provider.GetService(typeof(NotificationContext)));
            services.AddScoped(typeof(INotification), typeof(Notification));
        }
    }
}
