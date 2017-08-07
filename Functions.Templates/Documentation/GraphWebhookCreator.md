#### Settings for MS Graph GraphWebhookCreator Binding
The settings specify the following properties:

- `name` : The variable name used in function code for the GraphWebhookCreator object. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *GraphWebhookCreator*.
- `Listen`: MS Graph-specific resource to subscribe to (e.g. 'me/events')
- `changeType` : Kinds of changes function subscribes to.
- `PrincipalId` : Should be set to either an app setting containing the Principal ID/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal ID/OID
- `idToken` : Should be set to an expression that evaluates to an ID token. Either Principal ID or ID token must be set, but not both.
#### Example function.json
```json
{
  "bindings": [
    {
      "type": "httpTrigger",
      "name": "info",
      "authLevel": "anonymous",
      "methods": [
        "get",
        "post"
      ],
      "direction": "in"
    },
    {
      "type": "GraphWebhookCreator",
      "name": "creator",
      "changeType": [
        "Created"
      ],
      "Listen": "me/events",
      "PrincipalId": "",
      "idToken": "{idToken}",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
#r "O365Extensions"
#r "Microsoft.Graph"
#r "Newtonsoft.Json"

using Microsoft.Graph;
using Newtonsoft.Json;
using O365Extensions;

// Create a new Microsoft Graph subscription using an incoming 'idToken' parameter from an Http Trigger
public static async Task Run(UserInfo info, TraceWriter log, GraphWebhookCreator creator)
{
    var sub = await creator.SubscribeAsync();

    log.Info($"Created subscription with client state: {sub.ClientState}");
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "idToken", NullValueHandling = NullValueHandling.Ignore)]
    public string idToken { get; set; }
}
```

#### Supported types

Use the data type *GraphWebhookCreator* to create new subscriptions.
