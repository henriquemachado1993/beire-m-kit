using BeireMKit.Domain.BaseModels;

namespace BeireMKit.Notification.Interfaces
{
    public interface INotification
    {
        bool HasMessages { get; }
        bool HasErrors { get; }
        IReadOnlyCollection<MessageResult> Messages { get; }
        void AddMessage(MessageResult message);
        void AddMessages(IEnumerable<MessageResult> messages);
        void AddSuccess(string message, string? key = null, string ? typeCustom = null);
        void AddError(string message, string? key = null, string? typeCustom = null);
        void AddWarning(string message, string? key = null, string? typeCustom = null);
        void AddInfo(string message, string? key = null, string? typeCustom = null);
        void ClearMessages();
    }
}
