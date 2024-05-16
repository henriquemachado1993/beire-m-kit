using BeireMKit.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace BeireMKit.Cache.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public T Get<T>(string key)
        {
            var value = _distributedCache.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var serializedValue = JsonConvert.SerializeObject(value);
            _distributedCache.SetString(key, serializedValue, options);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
