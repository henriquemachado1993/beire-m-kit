# BeireMKit Cache
The BeireMKit Cache library was developed to make it easier to use the cache, with in-memory and distributed caching.

## Features
1. In-memory cache
2. Distributed cache (Redis)

## Requeriments
Make sure you have installed the .NET Core (6 or 8) SDK on your machine before you start.

## How to use

### MemoryCache
* Add cache configuration Envs
    ```Json
    {
        "CacheConfiguration": {
            "IsEnabled": true
        }
    }
    ```
* Add the cache service to Startup
	* In the ConfigureServices method of the Startup class, add the memory cache service: 
    ```
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.ConfigureMemoryCache(builder.Configuration);
    ```

### Distributed cache (Redis)
* Add cache configuration Envs
    ```Json
    {
        "CacheConfiguration": {
	        "UrlConnection": "https://redis.com",
            "IsEnabled": true
        }
    }
    ```
* Add the cache service to Startup
	* In the ConfigureServices method of the Startup class, add the memory/redis service: 
    ```
    var builder = WebApplication.CreateBuilder(args);
    builder.Services..ConfigureRedisCache(builder.Configuration);
    ```

## Usage example
	using BeireMKit.Cache.Interfaces;
	
    public class Service : IService
    {
        private readonly ICacheService _cache;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entity> _repository;

        public Service(
            ICacheService cache
            IUnitOfWork uow,
            IRepository<Entity> repository,
            )
        {
            _cache = cache;
            _uow = uow;
            _repository = repository;
        }

        public async Task<BaseResult<Entity>> GetSomethingAsync(int id, CancellationToken ct)
        {
            var cachedValue = await _cache.GetAsync(_cacheKey, ct);
            if(cachedValue is not null)
                return BaseResult<Entity>.CreateValidResult(cachedValue);
            
            var result = _repository.Get(id);
 
            _cache.Set<Entity>("key", result, TimeSpan.FromSeconds(60), ct)
 
            return BaseResult<Entity>.CreateValidResult(result);
        }

        public async Task<BaseResult<Entity>> GetSomethingAsync(int id, CancellationToken ct)
        {
            var cachedValue = await _cache.GetAsync(_cacheKey, async () => {
                return  _repository.Get(id);
            }, ct, TimeSpan.FromSeconds(60));
            
            return BaseResult<Entity>.CreateValidResult(cachedValue);
        }

        public async Task RemoveAsync(string key, CancellationToken ct)
        {
            await _cache.RemoveAsync(key, ct);
        }

        public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct)
        {
            await _cache.RemoveByPrefixAsync(prefix, ct);
        }
    }