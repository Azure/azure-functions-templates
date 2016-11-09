#### Settings for Service Bus queue or topic output binding

The settings specify the following properties.

- `name` : The variable name used in function code for the queue or queue message. 
- `queueName` : For queue trigger only, the name of the queue to poll.
- `topicName` : For topic trigger only, the name of the topic to poll.
- `subscriptionName` : For topic trigger only, the subscription name.
- `connection` : Same as for Service Bus trigger.
- `accessRights` : Specifies the access rights available for the connection string. Default value is `manage`. Set to `send` if you're using a connection string that doesn't provide manage permissions. Otherwise the Functions runtime might try and fail to do operations that require manage rights, such as creating queues.
- `type` : Must be set to *serviceBus*.
- `direction` : Must be set to *out*. 

#### Supported types

Azure Functions can create a Service Bus queue message from any of the following types.

* Object (always creates a JSON message, creates the message with a null object if the value is null when the function ends)
* string (creates a message if the value is non-null when the function ends)
* byte array (works like string) 
* `BrokeredMessage` (C#, works like string)

For creating multiple messages in a C# function, you can use `ICollector<T>` or `IAsyncCollector<T>`. A message is created when you call the `Add` method. For JavaScript, you can return an array.

#### C# code examples that create Service Bus queue messages

```csharp
public static void Run(TimerInfo myTimer, TraceWriter log, out string outputSbQueue)
{
	string message = $"Service Bus queue message created at: {DateTime.Now}";
    log.Info(message); 
    outputSbQueue = message;
}
```

```csharp
public static void Run(TimerInfo myTimer, TraceWriter log, ICollector<string> outputSbQueue)
{
	string message = $"Service Bus queue message created at: {DateTime.Now}";
    log.Info(message); 
    outputSbQueue.Add("1 " + message);
    outputSbQueue.Add("2 " + message);
}
```

#### JavaScript code example that creates Service Bus queue messages

```javascript
module.exports = function (context, myTimer) {
    var message = 'Service Bus queue message created at ' + timeStamp;
    context.log(message);   
    context.bindings.outputSbQueueMsg = message;
    context.done();
};
```

```javascript
module.exports = function (context, myTimer) {
    var message = 'Service Bus queue message created at ' + timeStamp;
    context.log(message);   
    context.bindings.outputSbQueueMsg = [
        message,
        {
            "hello":"world"
        }
    ];
    context.done();
};
```