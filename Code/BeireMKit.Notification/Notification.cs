using BeireMKit.Notification.Interfaces;

namespace BeireMKit.Notification
{
    public class Notification : INotification
    {
        private readonly List<string> _notifications;

        public Notification()
        {
            _notifications = new List<string>();
        }

        public bool HasNotifications => _notifications.Any();

        public IReadOnlyCollection<string> Notifications => _notifications.AsReadOnly();

        public void AddNotification(string message)
        {
            _notifications.Add(message);
        }

        public void AddNotifications(IEnumerable<string> messages)
        {
            _notifications.AddRange(messages);
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
        }
    }
}
