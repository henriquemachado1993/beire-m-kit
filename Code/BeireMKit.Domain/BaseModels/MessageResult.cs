using BeireMKit.Domain.Enums;

namespace BeireMKit.Domain.BaseModels
{
    public class MessageResult
    {
        public string? Key { get; set; }
        public string? Message { get; set; }
        public MessageType Type { get; set; }
        public string? TypeCustom { get; set; }
    }
}
