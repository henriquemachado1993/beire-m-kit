namespace BeireMKit.Cache.Configuration
{
    public interface ICacheConfiguration
    {
        string UrlConnection { get; set; }
        bool IsEnabled { get; set; }
    }
}
