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

#### Batch File Bindings for Storage Queue

When you define `queue` output bindings in a function each binding will be available to the batch via pre-assigned paths to temporary disk locations. You can inspect the values of these paths by using the `SET` command in your function and viewing the results in the logs, however these files are transient and only available during the scope of the function execution. Each variable defined will be accessible by the same name you provide in the Azure Functions interface.

For instance, an output binding you define as `outputQueueItem` will be created for you and populated with a temporary file path. You can access the path using the variable name surrounded by `%` characters such as `%outputQueueItem%`. 

The following code will test for the presence of the `name` parameter on the querystring of the request, and pipe that value to the `outputQueueItem` file, or return instructions by piping text to the response (in the `res` output location) if the `name` parameter is missing.

````
IF DEFINED req_query_name (
	echo Hello %req_query_name%! > %outputQueueItem%
	echo The greeting was sent to the queue. > %res%
) ELSE (
	echo Please pass a name on the query string > %res%
)
````

The contents of the file is then used by the runtime as the body of the message that is sent to the queue.

To pass additional values to the queue you can append to the file using the `>>` directive. 

````
echo This text is the first value > %outputQueueItem%
echo This text will be appended as a second line >> %outputQueueItem%
echo Careful! This line will overwrite previous data > %outputQueueItem%
````

Azure Functions do not currently provide a facility for serialization of data from a batch file (such as for JSON) so you need to take care of these operations through features natively available to you in batch file syntax. By echoing data and appending to the end of a file, you'll simply be pushing new lines of text to a text file. The values, along with the CRLF characters, will be passed as a single entry to the target queue.
