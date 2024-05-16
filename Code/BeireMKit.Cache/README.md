# BeireMKit Cache
The BeireMKit Cache library was developed to facilitate the use of cache

## Features
1. Cache in memory
2. Caching with Redis

## Requeriments
Make sure you have installed the .NET Core 6 SDK on your machine before you start.

## How to use
* Add the cache service to Startup
	* In the ConfigureServices method of the Startup class, add the memory/redis service: 
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureMemoryCache();
        // or
        services.ConfigureRedisCache();
    }
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

        public BaseResult<Entity> Get(int id)
        {
            if(_cache.KeyExists(_cacheKey))
                return BaseResult<Entity>.CreateValidResult(_cache.Get<Entity>("key"));
            
            var result = _repository.Get(id);
 
            _cache.Set<Entity>("key", result, TimeSpan.FromSeconds(60))
 
            return BaseResult<Entity>.CreateValidResult(result);
        }
    }