using BeireMKit.Notification.Interfaces;

namespace BeireMKit.Notification
{
    public class NotificationContext
    {
        private readonly Dictionary<string, INotification> _notifications;

        public NotificationContext()
        {
            _notifications = new Dictionary<string, INotification>();
        }

        public INotification GetNotification(string key)
        {
            if (!_notifications.TryGetValue(key, out var notification))
            {
                notification = new Notification();
                _notifications[key] = notification;
            }
            return notification;
        }

        public bool HasNotifications(string key)
        {
            return _notifications.ContainsKey(key) && _notifications[key].HasNotifications;
        }

        public IReadOnlyCollection<string> GetNotifications(string key)
        {
            return _notifications.ContainsKey(key) ? _notifications[key].Notifications : new List<string>();
        }

        public void ClearNotifications(string key)
        {
            if (_notifications.ContainsKey(key))
            {
                _notifications[key].ClearNotifications();
            }
        }

        public void ClearAllNotifications()
        {
            foreach (var notification in _notifications.Values)
            {
                notification.ClearNotifications();
            }
        }
    }
}
