using BeireMKit.Domain.BaseModels;
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

        public bool HasMessages(string key)
        {
            return _notifications.ContainsKey(key) && _notifications[key].HasMessages;
        }

        public bool HasErrors(string key)
        {
            return _notifications.ContainsKey(key) && _notifications[key].HasErrors;
        }

        public IReadOnlyCollection<MessageResult> GetNotifications(string key)
        {
            return _notifications.ContainsKey(key) ? _notifications[key].Messages : new List<MessageResult>();
        }

        public void ClearMessages(string key)
        {
            if (_notifications.ContainsKey(key))
            {
                _notifications[key].ClearMessages();
            }
        }

        public void ClearAllMessages()
        {
            foreach (var notification in _notifications.Values)
            {
                notification.ClearMessages();
            }
        }
    }
}
