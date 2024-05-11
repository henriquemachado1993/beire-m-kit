using BeireMKit.Domain.Enums;

namespace BeireMKit.Domain.BaseModels
{
    public class BaseResult<T>
    {
        public BaseResult() 
		{
		}
        public BaseResult(T data)
        {
            Data = data;
        }

        public T? Data { get; set; }
        public List<MessageResult> Messages { get; } = new List<MessageResult>();
        public string? Token { get; set; }

        public bool IsValid => !Messages.Any(x => x.Type == MessageType.Error);

        public static BaseResult<T> CreateValidResult(T? model = default)
        {
            return new BaseResult<T> { Data = model };
        }

        public static BaseResult<T> CreateValidResult(T? model = default, string? message = null)
        {
            var result = new BaseResult<T> { Data = model };
            result.AddError(message);
            return result;
        }

        public static BaseResult<T> CreateInvalidResult(T? model = default, string? message = null, Exception? exception = null)
        {
            var result = new BaseResult<T> { Data = model };
            result.AddError(message, exception);
            return result;
        }

        public static BaseResult<T> CreateInvalidResult(T? model = default, List<MessageResult>? messages = null)
        {
            var result = new BaseResult<T> { Data = model };
            result.AddError(messages);
            return result;
        }

        public static BaseResult<T> CreateInvalidResult(List<MessageResult> messages)
        {
            var result = new BaseResult<T> { Data = default };
            result.AddError(messages);
            return result;
        }

        public void AddError(Exception? exception)
        {
            if (exception != null)
            {
                Messages.Add(new MessageResult
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
        }

        public void AddError(string? message, Exception? exception = null)
        {
            Messages.Add(new MessageResult
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Error
            });

            if (exception != null)
            {
                Messages.Add(new MessageResult
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
        }

        public void AddError(string? message, string typeCustom)
        {
            Messages.Add(new MessageResult
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Error,
                TypeCustom = typeCustom
            });
        }

        public void AddError(MessageResult message, Exception? exception = null)
        {
            if (message is not null)
                Messages.Add(message);

            if (exception != null)
            {
                Messages.Add(new MessageResult
                {
                    Key = Guid.NewGuid().ToString(),
                    Message = exception.Message,
                    Type = MessageType.Error
                });
            }
        }

        public void AddError(IEnumerable<MessageResult> message)
        {
            if(message is not null)
                Messages.AddRange(message);
        }

        public void AddSuccess(string message, string? typeCustom = null)
        {
            Messages.Add(new MessageResult
            {
                Key = Guid.NewGuid().ToString(),
                Message = message,
                Type = MessageType.Success,
                TypeCustom = typeCustom
            });
        }

        public void AddSuccess(MessageResult message)
        {
            if(message is not null)
                Messages.Add(message);
        }

        public void AddSuccess(IEnumerable<MessageResult> message)
        {
            if (message is not null)
                Messages.AddRange(message);
        }
    }
}
