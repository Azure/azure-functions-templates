Using Mobile Apps table binding requires setting up an app setting. See these docs to see the full details: [go to azure.com](https://azure.microsoft.com/en-us/documentation/articles/functions-bindings-mobile-apps/#create-an-environment-variable-for-your-mobile-app-backend-url)

#### Settings for Mobile Apps input binding

The *function.json* file supports the following properties:

- `name` : Variable name used in function code for the new record.
- `type` : Biding type must be set to *mobileTable*.
- `tableName` : The table where the new record will be created.
- `id` : The ID of the record to retrieve. This property supports bindings similar to `{queueTrigger}`, which will use the string value of the queue message as the record Id.
- `apiKey` : String that is the application setting that specifies the optional API key for the mobile app. This is required when your mobile app uses an API key to restrict client access.
- `connection` : String that is the name of the environment variable in application settings that specifies the URL of your mobile app backend.
- `direction` : Binding direction, which must be set to *in*.

#### Azure Mobile Apps code example for a C# queue trigger

The input binding retrieves the record from a Mobile Apps table endpoint with the ID that matches the queue message string and passes it to the *record* parameter. When the record is not found, the parameter is null. The record is then updated with the new *Text* value when the function exits.

```csharp
#r "Newtonsoft.Json"	
using Newtonsoft.Json.Linq;

public static void Run(string myQueueItem, JObject record)
{
    if (record != null)
    {
        record["Text"] = "This has changed.";
    }    
}
```

#### Azure Mobile Apps code example for a JavaScript queue trigger

The input binding retrieves the record from a Mobile Apps table endpoint with the ID that matches the queue message string and passes it to the *record* parameter. In JavaScript functions, updated records are not sent back to the table. This code example writes the retrieved record to the log.

```javascript
module.exports = function (context, input) {    
    context.log(context.bindings.record);
    context.done();
};
```
