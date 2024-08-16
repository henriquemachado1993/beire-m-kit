using Microsoft.Extensions.Configuration;

namespace BeireMKit.Cache.Configuration
{
    public class CacheConfiguration : ICacheConfiguration
    {
        private const string CacheSection = "CacheConfiguration";

        public string UrlConnection { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;

        public static ICacheConfiguration Bind(IConfiguration configuration)
        {
            var cacheConfiguration = configuration.GetSection(CacheSection).Get<CacheConfiguration>();
            if (cacheConfiguration == null)
            {
                return new CacheConfiguration()
                {
                    IsEnabled = true,
                    UrlConnection = string.Empty
                };
            }
            return cacheConfiguration;
        }
    }
}
