namespace BeireMKit.Domain.BaseModels
{
    public class BaseFilter<T>
    {
        public BaseFilter()
        {
            CurrentPage = 1;
            Limit = 10;
        }

        public BaseFilter(T? filter, int currentPage = 1, int limit = 10)
        {
            Filter = filter;
            CurrentPage = currentPage > 0 ? currentPage : 1;
            Limit = limit > 0 ? limit : 10;
        }

        public T? Filter { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value > 0 ? value : 1;
        }

        private int _limit;
        public int Limit
        {
            get => _limit;
            set => _limit = value > 0 ? value : 10;
        }

        public static BaseFilter<T?> CreateValidResult(T? filter = default, int currentPage = 1, int limit = 10)
        {
            return new BaseFilter<T?>(filter, currentPage, limit);
        }

        public void UpdateCurrentPage(int? currentPage)
        {
            CurrentPage = currentPage.GetValueOrDefault(1);
        }
    }
}
