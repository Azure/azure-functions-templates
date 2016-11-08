#### Settings for storage blob trigger

- `name` : The variable name used in function code for the blob. 
- `path` : A path that specifies the container to monitor, and optionally a blob name pattern.
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the trigger will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *blobTrigger*.
- `direction` : Must be set to *in*.

#### Blob trigger name patterns

You can specify a blob name pattern in the `path` property. For example:

```json
"path": "input/original-{name}",
```

This path would find a blob named *original-Blob1.txt* in the *input* container, and the value of the `name` variable in function code would be `Blob1`.

Another example:

```json
"path": "input/{blobname}.{blobextension}",
```

This path would also find a blob named *original-Blob1.txt*, and the value of the `blobname` and `blobextension` variables in function code would be *original-Blob1* and *txt*.

You can restrict the types of blobs that trigger the function by specifying a pattern with a fixed value for the file extension. If you set the `path` to  *samples/{name}.png*, only *.png* blobs in the *samples* container will trigger the function.

If you need to specify a name pattern for blob names that have curly braces in the name, double the curly braces. For example, if you want to find blobs in the *images* container that have names like this:

		{20140101}-soundfile.mp3

use this for the `path` property:

		images/{{20140101}}-{name}

In the example, the `name` variable value would be *soundfile.mp3*. 

#### Blob trigger supported types

The blob can be deserialized to any of the following types in JavaScript or C# functions:

* Object (from JSON)
* String

In C# functions you can also bind to any of the following types:

* `TextReader`
* `Stream`
* `ICloudBlob`
* `CloudBlockBlob`
* `CloudPageBlob`
* `CloudBlobContainer`
* `CloudBlobDirectory`
* `IEnumerable<CloudBlockBlob>`
* `IEnumerable<CloudPageBlob>`
* Other types deserialized by [ICloudBlobStreamBinder](https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk-storage-blobs-how-to/)

#### Blob trigger C# code example

```csharp
public static void Run(string myBlob, TraceWriter log)
{
    log.Info($"C# Blob trigger function processed: {myBlob}");
}
```

#### Blob trigger JavaScript example

```JavaScript
module.exports = function(context, myBlob) {
    context.log(myBlob);
    context.done();
}
```