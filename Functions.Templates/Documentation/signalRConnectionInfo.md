#### Settings for SignalRConnectionInfo input binding

The settings specifies the following properties.

- `name` :Variable name used in function code for connection info object.
- `hubName` : This value must be set to the name of the SignalR hub for which the connection information is generated.
- `userId` : (Optional) The value of the user identifier claim to be set in the access key token.
- `connectionStringSetting` : The name of the app setting that contains the SignalR Service connection string (defaults to "AzureSignalRConnectionString")
- `type` : Must be set to *signalRConnectionInfo*.
- `direction` : Must be set to *in*. 

#### C# types for SignalRConnectionInfo input binding

The `SignalRConnectionInfo` binding uses provides its data in a `SignalRConnectionInfo` object.

#### C# Example for SignalRConnectionInfo input binding

This C# code example uses the binding to generate a valid connection info object and returns it in a web request.

```csharp
[FunctionName("negotiate")]
public static SignalRConnectionInfo Negotiate(
    [HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequest req,
    [SignalRConnectionInfo(HubName = "chat")]SignalRConnectionInfo connectionInfo)
{
    return connectionInfo;
}
```

#### JavaScript example for SignalRConnectionInfo input binding

This JavaScript code example uses the binding to generate a valid connection info object and returns it in a web request.

```JavaScript
module.exports = async function (context, req, connectionInfo) {
    // connectionInfo contains an access key token with a name identifier
    // claim set to the authenticated user
    context.res.body = connectionInfo;
};
```

For more information, see the full refernence for the binding in the [Azure Functions documentation](https://docs.microsoft.com/azure/azure-functions/functions-bindings-signalr-service#signalr-connection-info-input-binding).
