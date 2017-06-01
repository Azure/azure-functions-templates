#### Settings for Service Bus queue or topic trigger

The settings specify the following properties.

- `name` : The variable name used in function code for the queue or topic, or the queue or topic message. 
- `queueName` : For queue trigger only, the name of the queue to poll.
- `topicName` : For topic trigger only, the name of the topic to poll.
- `subscriptionName` : For topic trigger only, the subscription name.
- `connection` : The name of an app setting that contains a Service Bus connection string. The connection string must be for a Service Bus namespace, not limited to a specific queue or topic. If the connection string doesn't have manage rights, set the `accessRights` property. If you leave `connection` empty, the trigger or binding will work with the default Service Bus connection string for the function app, which is specified by the AzureWebJobsServiceBus app setting.
- `accessRights` : Specifies the access rights available for the connection string. Default value is `manage`. Set to `listen` if you're using a connection string that doesn't provide manage permissions. Otherwise the Functions runtime might try and fail to do operations that require manage rights.
- `type` : Must be set to *serviceBusTrigger*.
- `direction` : Must be set to *in*. 

#### C# code example that processes a Service Bus queue message

```csharp
public static void Run(string myQueueItem, TraceWriter log)
{
    log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
}
```

#### JavaScript code example that processes a Service Bus queue message

```javascript
module.exports = function(context, myQueueItem) {
    context.log('JavaScript ServiceBus queue trigger function processed message', myQueueItem);
    context.done();
};
```

#### Supported types

The Service Bus queue message can be deserialized to any of the following types:

* Object (from JSON)
* string
* byte array 
* `BrokeredMessage` (C#) 
