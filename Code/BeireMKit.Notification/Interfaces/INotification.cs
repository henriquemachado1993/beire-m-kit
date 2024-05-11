using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Notification.Interfaces
{
    public interface INotification
    {
        bool HasNotifications { get; }
        IReadOnlyCollection<string> Notifications { get; }
        void AddNotification(string message);
        void AddNotifications(IEnumerable<string> messages);
        void ClearNotifications();
    }
}
