#### Settings for storage queue output binding

The settings specifies the following properties.

- `name` : The variable name used in function code for the queue or the queue message. 
- `queueName` : The name of the queue. For queue naming rules, see [Naming Queues and Metadata](https://msdn.microsoft.com/library/dd179349.aspx).
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the trigger will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *queue*.
- `direction` : Must be set to *out*. 

#### C# types for Storage Queue

The `queue` binding can serialize the following types to a queue message:

* Object (`out T` in C#, creates a message with a null object if the parameter is null when the function ends)
* String (`out string` in C#, creates queue message if parameter value is non-null when the function ends)
* Byte array (`out byte[]` in C#, works like string) 
* `out CloudQueueMessage` (C#, works like string) 

In C# you can also bind to `ICollector<T>` or `IAsyncCollector<T>` where `T` is one of the supported types.

#### C# Example for Storage Queue

This C# code example writes a single output queue message for each input queue message.

```csharp
public static void Run(string myQueueItem, out string myOutputQueueItem, TraceWriter log)
{
    myOutputQueueItem = myQueueItem + "(next step)";
}
```

This C# code example writes multiple messages by using  `ICollector<T>` (use `IAsyncCollector<T>` in an async function):

```csharp
public static void Run(string myQueueItem, ICollector<string> myQueue, TraceWriter log)
{
    myQueue.Add(myQueueItem + "(step 1)");
    myQueue.Add(myQueueItem + "(step 2)");
}
```

#### JavaScript example for Storage Queue

JavaScript supports outputing a single object/string or an array of objects/strings. You access your bindings on `context.bindings` object.

This shows outputing a single item:

```JavaScript
module.exports = function(context, myQueueItem) {
    context.bindings.output = "Azure Functions are awesome!"
    //objects also work:
    //context.bindings.output = { "name": "world"}
    context.done();
}
```

This shows outputing a collection of items:

```JavaScript
module.exports = function(context, myQueueItem) {
    context.bindings.output = [
        {
            "name":"world"
        },
        "Azure Functions are awesome!"
    ]
    context.done();
}
```