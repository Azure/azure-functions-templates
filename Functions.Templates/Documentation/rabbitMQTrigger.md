#### Settings for RabbitMQ trigger

The settings specify the following properties.

- `queueName` : The name of the queue to listen to.
- `connectionStringSetting` : The name of an app setting that has connection string.
- `hostName` : Host name to authenticate with in order to listen to queue.
- `userNameSetting` : App setting that contains user name to authenticate with in order to listen to queue.
- `passwordSetting` : App setting that contains password to authenticate with in order to listen to queue.
- `port` : Port to attach to. Defaults to 0.
- `type` : Must be set to *rabbitMqTrigger*.
- `direction` : Must be set to *in*. 

#### C# code example that processes a RabbitMQ queue message

```csharp
public static void Run([RabbitMQTrigger("NameOfQueue", ConnectionStringSetting = "ConnectionValue")]string myQueueItem, ILogger log)
{
	log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
}
```

#### JavaScript code example that processes a RabbitMQ queue message

```javascript
module.exports = async function (context, myQueueItem) {
    context.log('JavaScript rabbitmq trigger function processed work item', myQueueItem);
};
```

#### Supported types

The RabbitMQ queue message can be deserialized to any of the following types:

* string
* POCO
* byte array
* BasicDeliverEventArgs (C#)