#### Settings for RabbitMQ trigger

The settings specify the following properties.

- `queueName` : The name of the queue to listen to.
- `connectionStringSetting` : The name of an app setting that has connection string.

#### C# code example that processes a RabbitMQ queue message

```csharp
public static void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")]string myQueueItem, ILogger log)
{
	log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
}
```

#### Supported types

The RabbitMQ queue message can be deserialized to any of the following types:

* string
* POCO
* byte array
* BasicDeliverEventArgs