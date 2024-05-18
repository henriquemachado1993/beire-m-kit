using Microsoft.Extensions.Configuration;

namespace BeireMKit.Domain.Helpers
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static T Get<T>(string key)
        {
            ValidateConfiguration();
            return _configuration.GetSection(key).Get<T>();
        }

        public static T GetChild<T>(string sessionName, string key)
        {
            ValidateConfiguration();
            return _configuration.GetSection(sessionName).GetSection(key).Get<T>();
        }

        private static void ValidateConfiguration()
        {
            if (_configuration == null)
                throw new ArgumentNullException(nameof(_configuration), "Configuration has not been initialized. Call Initialize method first.");
        }
    }
}
