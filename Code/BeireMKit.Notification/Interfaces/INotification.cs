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
