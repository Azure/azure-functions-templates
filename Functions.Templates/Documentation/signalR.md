#### Settings for SignalR output binding

The settings specifies the following properties.

- `name` : Variable name used in function code for connection info object.
- `hubName` : This value must be set to the name of the SignalR hub for which the connection information is generated.
- `connectionStringSetting` : The name of the app setting that contains the SignalR Service connection string (defaults to "AzureSignalRConnectionString")
- `type` : Must be set to *signalR*.
- `direction` : Must be set to *out*. 

#### C# types for SignalR output binding

The `signalR` binding can serialize the following types to a SignalR message:

* `SignalRMessage`
* `SignalRGroupAction`

In C# you can also bind to `ICollector<T>` or `IAsyncCollector<T>` where `T` is one of the supported types.

#### C# Example for SignalR output binding

This C# code example sends a message to all clients connected to a SignalR hub named `chat`.

```csharp
public static Task SendMessage(object message, IAsyncCollector<SignalRMessage> signalRMessages)
{
    return signalRMessages.AddAsync(
        new SignalRMessage 
        {
            Target = "newMessage", 
            Arguments = new [] { message } 
        });
}
```

#### JavaScript example for SignalR output binding

JavaScript supports outputing a single object/string or an array of objects/strings. You access your bindings on `context.bindings` object.

This shows outputing a single item:

```JavaScript
module.exports = async function (context, req) {
    context.bindings.signalRMessage = {
        "target": "newMessage",
        "arguments": [ req.body ]
    };
};
```

For more information, see the full refernence for the binding in the [Azure Functions documentation](https://docs.microsoft.com/azure/azure-functions/functions-bindings-signalr-service#signalr-output-binding).
