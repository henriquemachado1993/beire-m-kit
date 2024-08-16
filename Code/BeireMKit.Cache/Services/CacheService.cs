using BeireMKit.Cache.Configuration;
using BeireMKit.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BeireMKit.Cache.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, bool> _cacheKeys = new();
        private readonly IDistributedCache _distributedCache;
        private readonly ICacheConfiguration _cacheConfiguration;
        private readonly ILogger<CacheService> _logger;

        public CacheService(
            IDistributedCache distributedCache,
            ICacheConfiguration cacheConfiguration,
            ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _cacheConfiguration = cacheConfiguration;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            if (!_cacheConfiguration.IsEnabled)
            {
                _logger.LogDebug("[CacheService] cache is disabled");
                return null;
            }

            string? cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cacheValue == null)
            {
                _logger.LogDebug("[CacheService] Key {Key} not found in cache", key);
                return null;
            }
            _logger.LogDebug("[CacheService] Key {Key} found in cache", key);
            return JsonConvert.DeserializeObject<T>(cacheValue);
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default, TimeSpan expiration = default) where T : class
        {
            T? cachedValue = await GetAsync<T>(key, cancellationToken);

            if (cachedValue != null)
            {
                _logger.LogDebug("[CacheService] Key {Key} found in cache", key);
                return cachedValue;
            }

            cachedValue = await factory();
            await SetAsync(key, cachedValue, expiration);

            _logger.LogDebug("[CacheService] cache has been set for key {Key}, expiration {expiration}", key, expiration);

            return cachedValue;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default) where T : class
        {
            if (!_cacheConfiguration.IsEnabled)
            {
                _logger.LogDebug("[CacheService] cache is disabled");
                return;
            }

            string cacheValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(
                key,
                cacheValue,
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = expiration
                }
            );
            _cacheKeys.TryAdd(key, false);
            _logger.LogDebug("[CacheService] cache has been set for key {Key}, expiration {expiration}", key, expiration);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if (!_cacheConfiguration.IsEnabled)
            {
                _logger.LogDebug("[CacheService] cache is disabled");
                return;
            }

            await _distributedCache.RemoveAsync(key, cancellationToken);
            _cacheKeys.TryRemove(key, out bool _);
            _logger.LogDebug("[CacheService] cache has been removed for key {Key}", key);
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            if (!_cacheConfiguration.IsEnabled)
            {
                _logger.LogDebug("[CacheService] cache is disabled");
                return;
            }

            IEnumerable<Task> tasks = _cacheKeys
                .Keys
                .Where(key => key.StartsWith(prefixKey))
                .Select(key => RemoveAsync(key, cancellationToken));
            await Task.WhenAll(tasks);
            _logger.LogDebug("[CacheService] The caches have been removed for the {PrefixKey} prefix", prefixKey);
        }
    }
}
