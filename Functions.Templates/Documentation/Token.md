#### Settings for Token Input Binding

The settings specify the following properties:

- `name` : The variable name used in function code to identify the token.
- `type` : Must be set to *token*.
- `direction` : Must be set to *in*. 
- `Resource` : Resource to retrieve an authentication token for. Default is Microsoft Graph.
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with the specific Resource or an expression to evaluate to a Principal id/OID
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.

#### Example bindings.json
```json
{
  "bindings": [
    {
      "type": "httpTrigger",
      "name": "info",
      "authLevel": "anonymous",
      "methods": [
        "get",
        "post",
      ],
      "direction": "in"
    },   
    {
      "type": "token",
      "name": "token",
      "Resource": "https://graph.windows.net",
      "idToken": {idToken}
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
#r "Newtonsoft.Json"

using Newtonsoft.Json;

public static async Task Run(UserInfo info, TraceWriter log, string token)
{
    log.Info($"Retrieved AD Graph token: {token}");
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "idToken", NullValueHandling = NullValueHandling.Ignore)]
    public string idToken { get; set; }
}
```

#### Supported types

Token will be input in the form of a *string*