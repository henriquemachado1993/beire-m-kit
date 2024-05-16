using BeireMKit.Domain.BaseModels;
using BeireMKit.Domain.Enums;
using BeireMKit.Notification.Interfaces;

namespace BeireMKit.Notification
{
    public class Notification : INotification
    {
        private readonly List<MessageResult> _messages;

        public Notification()
        {
            _messages = new List<MessageResult>();
        }

        public bool HasMessages => _messages.Any();
        public bool HasErrors => _messages.Any(x => x.Type == MessageType.Error);

        public IReadOnlyCollection<MessageResult> Messages => _messages.AsReadOnly();

        public void AddMessage(MessageResult message)
        {
            _messages.Add(message);
        }

        public void AddMessages(IEnumerable<MessageResult> messages)
        {
            _messages.AddRange(messages);
        }

        public void AddSuccess(string message, string? key = null, string? typeCustom = null)
        {
            AddMessage(message, MessageType.Success, key, typeCustom);
        }

        public void AddError(string message, string? key = null, string? typeCustom = null)
        {
            AddMessage(message, MessageType.Error, key, typeCustom);
        }

        public void AddInfo(string message, string? key = null, string? typeCustom = null)
        {
            AddMessage(message, MessageType.Info, key, typeCustom);
        }

        public void AddWarning(string message, string? key = null, string? typeCustom = null)
        {
            AddMessage(message, MessageType.Warning, key, typeCustom);
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        private void AddMessage(string message, MessageType type, string? key = null, string? typeCustom = null)
        {
            _messages.Add(new MessageResult()
            {
                Key = key ?? Guid.NewGuid().ToString(),
                Message = message,
                Type = type,
                TypeCustom = typeCustom
            });
        }
    }
}
