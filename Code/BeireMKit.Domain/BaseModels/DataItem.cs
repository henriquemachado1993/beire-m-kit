namespace BeireMKit.Domain.BaseModels
{
    public class DataItem<T>
    {
        public string Key { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
