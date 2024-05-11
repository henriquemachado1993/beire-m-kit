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

        public static PagingResult<T> CreateValidResultPaging(T model, PageResult pageResult)
        {
            var pagingResult = new PagingResult<T>(model);
            pagingResult.Paging = pageResult;
            return pagingResult;
        }

        public static PagingResult<T?> CreateValidResultPaging()
        {
            return new PagingResult<T?>(default);
        }

        public static PagingResult<T?> CreateInvalidResultPaging(List<MessageResult> message)
        {
            var pagingResult = new PagingResult<T?>(default);
            pagingResult.AddError(message);
            return pagingResult;
        }

        public static PagingResult<T?> CreateInvalidResultPaging(MessageResult message)
        {
            var pagingResult = new PagingResult<T?>(default);
            pagingResult.AddError(message);
            return pagingResult;
        }
    }
}
