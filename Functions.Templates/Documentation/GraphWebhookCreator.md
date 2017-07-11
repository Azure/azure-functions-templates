#### Settings for MS Graph GraphWebhookCreator Binding
The settings specify the following properties:

- `name` : The variable name used in function code for the GraphWebhookCreator object. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *GraphWebhookCreator*.
- `Listen`: MS Graph-specific resource to subscribe to (e.g. 'me/events')
- `changeType` : Kinds of changes function subscribes to.
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal id/OID
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.

#### C# Example code
```csharp
#r "O365Extensions"
#r "Microsoft.Graph"
#r "Newtonsoft.Json"

using Microsoft.Graph;
using Newtonsoft.Json;
using O365Extensions;

// Create a new Microsoft Graph subscription using an incoming 'userId' parameter from an Http Trigger
public static async Task Run(UserInfo info, TraceWriter log, GraphWebhookCreator g)
{
    var sub = await g.SubscribeAsync();

    log.Info($"Created subscription with client state: {sub.ClientState} using User ID: {info.UserId}");
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
    public string UserId { get; set; }
}
```

#### Supported types

Use the data type *GraphWebhookCreator* to create new subscriptions.
