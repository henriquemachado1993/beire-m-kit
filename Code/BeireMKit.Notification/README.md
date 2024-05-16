
# BeireMKit Notification
The BeireMKit Notification library was developed to facilitate the handling of messages in the project, using the Notification Pattern.

## Features

1. handle system messages in a simplified way.
2. deal with different message contexts

## Requirements
Make sure you have installed the .NET Core 6 SDK on your machine before you start.

## How to use
* Add the notification service to Startup
	* In the ConfigureServices method of the Startup class, add the notification service using the ConfigureNotification() method:
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureNotification();
		services.ConfigureNotificationContext();
    }
    ```
 ## Usage example
 * Services example
	```
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
			// Types
	        _notification.AddSuccess("deletion success", "key to identifying msg", "success-custom");
	        _notification.AddError("deletion error", "key to identifying msg", "error-custom");
	        _notification.AddInfo("deletion info", "key to identifying msg", "info-custom");
	        _notification.AddWarning("deletion warning", "key to identifying msg", "warning-custom");
	        
	        // clear messages
	        _notification.ClearMessages();
	        
	        // verification
	        _notification.HasMessages;
	        _notification.HasErrors;
	    }
	}
	```
 * Controller example
	```
	[HttpDelete("delete/{id}")]
	public IActionResult Delete([FromRoute] int id)
	{
		_service.Delete(id);
		
	    if (_notification.HasMessages)
	    {
		  return Ok(_notification.Messages);
	    }
	    if (_notification.HasErrors)
	    {
		  return Ok(_notification.Messages.Any(x => x.Type == MessageType.Error));
	    }
		return Ok();
	}
	```
 * notification context example
 	```
	using BeireMKit.Notification;

	public class OrderService
	{
		private readonly NotificationContext _notificationContext;

		public OrderService(NotificationContext notificationContext)
		{
			_notificationContext = notificationContext;
		}

		public void CreateOrder()
		{
			var orderNotification = _notificationContext.GetNotification("OrderCreated");

			// Another context
			var orderCancelNotification = _notificationContext.GetNotification("OrderCancel");

			try
			{
				// code..

				orderNotification.AddSuccess("Order was created successfully.");
			}
			catch (Exception ex)
			{
				orderNotification.AddError($"Error creating order: {ex.Message}");
			}
		}

		public bool HasOrderCreationErrors()
		{
			return _notificationContext.HasErrors("OrderCreated");
		}

		public IReadOnlyCollection<MessageResult> GetOrderCreationMessages()
		{
			return _notificationContext.GetNotifications("OrderCreated");
		}

		public void ClearOrderCreationMessages()
		{
			_notificationContext.ClearMessages("OrderCreated");
		}
	}

	```