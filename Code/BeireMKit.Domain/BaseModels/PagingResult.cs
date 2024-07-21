using System.Net;

namespace BeireMKit.Domain.BaseModels
{
    public class PagingResult<T> : BaseResult<T>
    {
        public PageResult Paging { get; set; }

        public PagingResult(T data) : base(data)
        {
            Data = data;
            Paging = new PageResult();
        }

        public static PagingResult<T> CreateValidResultPaging(T model, PageResult pageResult, string? message = null)
        {
            var pagingResult = new PagingResult<T>(model);
            pagingResult.Paging = pageResult;
            pagingResult.AddSuccess(message);
            return pagingResult;
        }

        public static PagingResult<T?> CreateValidResultPaging()
        {
            return new PagingResult<T?>(default);
        }

        public static PagingResult<T?> CreateInvalidResultPaging(List<MessageResult> message, PageResult? pageResult = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var pagingResult = new PagingResult<T?>(default);

            if (pageResult != null)
                pagingResult.Paging = pageResult;

            pagingResult.AddError(message);
            pagingResult.StatusCode = statusCode;
            return pagingResult;
        }

        public static PagingResult<T?> CreateInvalidResultPaging(MessageResult message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var pagingResult = new PagingResult<T?>(default);
            pagingResult.AddError(message);
            pagingResult.StatusCode = statusCode;
            return pagingResult;
        }
    }
}
