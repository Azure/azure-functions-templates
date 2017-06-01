Using Mobile Apps table binding requires setting up an app setting. See these docs to see the full details: [go to azure.com](https://azure.microsoft.com/en-us/documentation/articles/functions-bindings-mobile-apps/#create-an-environment-variable-for-your-mobile-app-backend-url)

#### Settings for Mobile Apps output binding

The settings support the following properties:

- `name` : Variable name used in function code for the new record.
- `type` : Binding type that must be set to *mobileTable*.
- `tableName` : The table where the new record is created.
- `apiKey` : String that is the application setting that specifies the optional API key for the mobile app. This is required when your mobile app uses an API key to restrict client access.
- `connection` : String that is the name of the environment variable in application settings that specifies the URL of your mobile app backend.
- `direction` : Binding direction, which must be set to *out*.

#### Azure Mobile Apps code example for a C# queue trigger

This C# code example inserts a new record into a Mobile Apps table endpoint with a *Text* property into the table specified in the above binding.

```csharp
public static void Run(string myQueueItem, out object record)
{
    record = new {
        Text = $"I'm running in a C# function! {myQueueItem}"
    };
}
```

#### Azure Mobile Apps code example for a JavaScript queue trigger

This JavaScript code example inserts a new record into a Mobile Apps table endpoint with a *text* property into the table specified in the above binding.

```javascript
module.exports = function (context, input) {

    context.bindings.record = {
        text : "I'm running in a JavaScript function! Data: '" + input + "'"
    }   

    context.done();
};
```