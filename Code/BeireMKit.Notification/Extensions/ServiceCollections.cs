using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeireMKit.Notification.Interfaces;

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
            services.AddScoped(typeof(INotification), provider => provider.GetService(typeof(NotificationContext)));
        }
    }
}
