#### Settings for a storage blob output binding

- `name` : The variable name used in function code for the blob . 
- `path` : A path that specifies the container to write the blob to, and optionally a blob name pattern.
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the binding will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *blob*.
- `direction` : Set to *out*. 

#### Blob input and output supported types

The `blob` binding can serialize or deserialize the following types in JavaScript or C# functions:

* Object (`out T` in C# for output blob: creates a blob as null object if parameter value is null when the function ends)
* String (`out string` in C# for output blob: creates a blob only if the string parameter is non-null when the function returns)

In C# functions, you can also bind to the following types:

* `TextWriter`
* `Stream`
* `CloudBlobStream`
* `ICloudBlob`
* `CloudBlockBlob` 
* `CloudPageBlob` 

#### Blob output C# code example

```csharp
public static void Run(string myQueueItem, string myInputBlob, out string myOutputBlob, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    myOutputBlob = myInputBlob;
}
```

#### Blob output JavaScript example

```JavaScript
module.exports = function(context, trigger) {
    context.bindings.output = { "hello":"world" }
    context.done();
}
```