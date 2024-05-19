
# BeireMKit Data
The BeireMKit Data library was developed to facilitate interaction with databases, using the Repository, UnitOfWork and a BaseMap for OnModelCreating standards. This documentation will guide you on how to use the functionalities offered by the library.

## Features
1. manipulate data from the database in a simplified way.
2. Perform queries.
3. Control transactions.

## Requirements
Make sure you have installed the .NET Core 6 SDK on your machine before you start.

## How to use Relational Repository
* Add the repository service to Startup
	* In the ConfigureServices method of the Startup class, add the repository service using the ConfigureRepository() method: 
	    ```
	    public void ConfigureServices(IServiceCollection services)
	    {
	        services.ConfigureRepository();
	    }
	    ```
    
* Configuring DbContext
	* Add IBaseContext to your context: Make sure that your context class (YourContext) implements the IBaseDbContext interface. 
	    ```
	    public class YourContext : DbContext, IBaseDbContext
	    {
	        protected override void OnModelCreating(ModelBuilder builder)
			{
			    base.OnModelCreating(builder);
			    builder.ApplyConfiguration(new EntityMap());
			}
	    }
	    ```
    * Add YourMap.
	    ```
	    using BeireMKit.Data.Map;
		using Microsoft.EntityFrameworkCore;
		using Microsoft.EntityFrameworkCore.Metadata.Builders;
		
	    public class EntityMap : MapBase<Entity>
		{
		    public override void Configure(EntityTypeBuilder<Entity> builder)
		    {
		        base.Configure(builder);
		        builder.ToTable("TableName");
		    }
		}
	    ```
	* Add dependency injection: In the ConfigureServices method of the Startup class, add the dependency injection for your context:
	
	    ```
	    public void ConfigureServices(IServiceCollection services)
	    {
	        services.AddScoped<IBaseDbContext, YourContext>();
	    }
	    ```
    * Usage example
	    ```
	    using BeireMKit.Data.Interfaces;
		using BeireMKit.Domain.BaseModels;
		using BeireMKit.Domain.Extensions;
		
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
## How to use Non-relational repository MongoDB
* Add the repository service to Startup
	* In the ConfigureServices method of the Startup class, add the repository service using the ConfigureRepository() method: 
	    ```
	    public void ConfigureServices(IServiceCollection services)
	    {
	        var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
			builder.Services.AddSingleton(mongoDBSettings);
			builder.Services.AddScoped<IBaseMongoDbContext, MongoDBContext>();
			builder.Services.ConfigureMongoDbRepository();
	    }
	    ```
    
* Configuring DbContext
	* Add IBaseContext to your context: Make sure that your context class (YourContext) implements the IBaseDbContext interface. 
	    ```
	    public class YourContext : DbContext, IBaseDbContext
	    {
	        protected override void OnModelCreating(ModelBuilder builder)
			{
			    base.OnModelCreating(builder);
			    builder.ApplyConfiguration(new EntityMap());
			}
	    }
	    ```
    * Add YourMap.
	    ```
	    using BeireMKit.Data.Map;
		using Microsoft.EntityFrameworkCore;
		using Microsoft.EntityFrameworkCore.Metadata.Builders;
		
	    public class EntityMap : MapBase<Entity>
		{
		    public override void Configure(EntityTypeBuilder<Entity> builder)
		    {
		        base.Configure(builder);
		        builder.ToTable("TableName");
		    }
		}
	    ```
	* Add dependency injection: In the ConfigureServices method of the Startup class, add the dependency injection for your context:
	
	    ```
	    public void ConfigureServices(IServiceCollection services)
	    {
	        services.AddScoped<IBaseDbContext, YourContext>();
	    }
	    ```
    * Usage example
	    ```
	    using BeireMKit.Data.Interfaces;
		using BeireMKit.Domain.BaseModels;
		using BeireMKit.Domain.Extensions;
		
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