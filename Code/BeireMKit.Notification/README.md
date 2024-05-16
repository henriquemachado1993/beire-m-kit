# BeireMKit Notification
The BeireMKit Notification library was developed to facilitate the handling of messages in the project, using the Notification Pattern.

## Features

1. handling system messages in a simplified way.

## Requirements
Make sure you have installed the .NET Core 6 SDK on your machine before you start.

## How to use
* Add the notification service to Startup
	* In the ConfigureServices method of the Startup class, add the notification service using the ConfigureNotification() method:
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureNotification();
    }
    ```
 ## Usage example
	using BeireMKit.Notification.Interfaces;
	
    public class Service : IService
    {
        private readonly INotification _notification;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Entity> _repository;

        public Service(
            INotification notification
            IUnitOfWork uow,
            IRepository<Entity> repository,
            )
        {
            _notification = notification;
            _uow = uow;
            _repository = repository;
        }

        public void Delete(int? id)
        {
            if(id == null){
				_notification.AddNotification("Id cannot be null");
			}
            
            // code..
        }
    }
	
	// Controller example
	[HttpDelete("delete/{id}")]
	public IActionResult Delete([FromRoute] int id)
	{
	    _service.Delete(boResultProduct.Data.Id);
        if (_notification.HasNotifications)
        {
		    return Ok(_notification.Notifications);
        }
	    return Ok();
	}