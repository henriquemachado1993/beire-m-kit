# BeireMKit Data
A biblioteca BeireMKit Data foi desenvolvida para facilitar a interação com banco de dados, utilizando os padrões Repository, UnitOfWork e um BaseMap para OnModelCreating. Esta documentação vai te guiar sobre como usar as funcionalidades oferecidas pela biblioteca.

## Funcionalidades

1. Manipular dados do banco de dados de forma simplificada.
2. Realizar consultas.
3. Controlar transações.

## Pré-requisitos
Certifique-se de ter instalado o .NET Core 6 SDK em sua máquina antes de começar.

## Como Usar
* Adicionar o serviço do repositório no Startup
	* No método ConfigureServices da classe Startup, adicione o serviço do repositório usando o método ConfigureRepository(): 
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureRepository();
    }
    ```
    
* Configurar o DbContext
	* Adicionar IBaseContext ao seu contexto: Certifique-se de que sua classe de contexto (YourContext) implementa a interface IBaseDbContext.
    ```
    public class YourContext : DbContext, IBaseDbContext
    {
        // Seu código aqui
    }
    ```
	* Adicionar injeção de dependência: No método ConfigureServices da classe Startup, adicione a injeção de dependência para o seu contexto:
	
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IBaseDbContext, YourContext>();
    }
    ```
    * Exemplo de uso:
    ```
    public class Service : IService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entity> _repository;

        public Service(
            IUnitOfWork uow,
            IRepository<Entity> repository,
            )
        {
            _uow = uow;
            _repository = repository;
        }

        public BaseResult<Entity> Add(Entity entity)
        {
            var result = _repository.Add(entity);
            _uow.Commit();
            return BaseResult<Entity>.CreateValidResult(result);
        }
    }
    ```