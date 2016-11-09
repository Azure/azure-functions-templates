#### Settings for a storage blob input binding

- `name` : The variable name used in function code for the blob . 
- `path` : A path that specifies the container to read the blob from and optionally a blob name pattern.
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the binding will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *blob*.
- `direction` : Set to *in*

#### Blob input and output supported types

The `blob` binding can serialize or deserialize the following types in JavaScript or C# functions:

* Object
* String

In C# functions, you can also bind to the following types:

* `TextReader`
* `Stream`
* `ICloudBlob`
* `CloudBlockBlob` 
* `CloudPageBlob` 

#### Blob output C# code example

This C# code example copies a blob whose name is received in a queue message.

```csharp
public static void Run(string myQueueItem, string myInputBlob, out string myOutputBlob, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    myOutputBlob = myInputBlob;
}
```

#### Blob output JavaScript example

```JavaScript
// this assumes that your blob input is your only input
module.exports = function(context, trigger, inputBlob) {
    context.log(inputBlob);
    //it's also available on context.bindings
    context.log(context.bindings.inputBlob); // will log the same thing as above
    context.done();
}
```