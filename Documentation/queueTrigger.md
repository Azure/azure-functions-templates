#### Settings for storage queue trigger

- `name` : The variable name used in function code for the queue or the queue message. 
- `queueName` : The name of the queue to poll. For queue naming rules, see [Naming Queues and Metadata](https://msdn.microsoft.com/library/dd179349.aspx).
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the trigger will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *queueTrigger*.
- `direction` : Must be set to *in*. 


#### Additional metadata for Storage Queue trigger

You can get queue metadata in your function by using these variable names:

* ExpirationTime
* InsertionTime
* NextVisibleTime
* Id
* PopReceipt
* DequeueCount
* QueueTrigger (another way to retrieve the queue message text as a string)

#### C# types for Storage Queue trigger

The queue message can be deserialized to any of the following types:

* Object (from JSON)
* String
* Byte array 
* `CloudQueueMessage` (C#) 

#### C# example for Storage Queue trigger

```csharp
public static void Run(string myQueueItem, 
    DateTimeOffset expirationTime, 
    DateTimeOffset insertionTime, 
    DateTimeOffset nextVisibleTime,
    string queueTrigger,
    string id,
    string popReceipt,
    int dequeueCount,
    TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}\n" +
        $"queueTrigger={queueTrigger}\n" +
        $"expirationTime={expirationTime}\n" +
        $"insertionTime={insertionTime}\n" +
        $"nextVisibleTime={nextVisibleTime}\n" +
        $"id={id}\n" +
        $"popReceipt={popReceipt}\n" + 
        $"dequeueCount={dequeueCount}");
}
```

#### JavaScript example for Storage Queue trigger

```JavaScript
module.exports = function(context, myQueueItem) {
    context.log('Dequeue Count: ' + context.bindingData.DequeueCount);

    // JavaScript supports returning strings or objects for queues
    if(typeof myQueueItem === 'string') {
        context.log('Trigger was a string! - ' + myQueueItem);
    } else if(typeof myQueueItem === 'object') {
        context.log('Trigger was an object! - \n' + JSON.stringify(myQueueItem, null, ' '));
    }
    context.done(); //finish execution
}
```