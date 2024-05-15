
# BeireMKit Cache
A biblioteca BeireMKit Cache foi desenvolvida para facilitar a utilização de cache

## Funcionalidades

1. Cache em memória
2. Cache com Redis

## Pré-requisitos
Certifique-se de ter instalado o .NET Core 6 SDK em sua máquina antes de começar.

## Como Usar
* Adicionar o serviço do repositório no Startup
	* No método ConfigureServices da classe Startup, adicione o serviço de cache com memória/redis: 
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureMemoryCache();
        services.ConfigureRedisCache();
    }
    ```
    
## Exemplo de uso:
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
            var result = _cache.Get<Entity>("key");
            if(result != null)
                return BaseResult<Entity>.CreateValidResult(result);
            
            result = _repository.Get(id);
 
            _cache.Set<Entity>("key", result, TimeSpan.FromSeconds(60))
 
            return BaseResult<Entity>.CreateValidResult(result);
        }
    }