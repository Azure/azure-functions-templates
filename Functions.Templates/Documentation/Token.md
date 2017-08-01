#### Settings for Token Input Binding

The settings specify the following properties:

- `name` : The variable name used in function code to identify the token.
- `type` : Must be set to *token*.
- `direction` : Must be set to *in*. 
- `Resource` : Resource to retrieve an authentication token for. Default is Microsoft Graph.
- `PrincipalId` : Should be set to either an app setting containing the Principal ID/OID to be used to communicate with the specific Resource or an expression to evaluate to a Principal ID/OID
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
        "post",
      ],
      "direction": "in"
    },   
    {
      "type": "token",
      "name": "token",
      "Resource": "https://graph.windows.net",
      "PrincipalId": "{principalId}",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example Code
```csharp
#r "Newtonsoft.Json"

using Newtonsoft.Json;

public static async Task Run(UserInfo info, TraceWriter log, string token)
{
    log.Info($"Retrieved AD Graph token: {token}");
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "principalId", NullValueHandling = NullValueHandling.Ignore)]
    public string idToken { get; set; }
}
```

#### JavaScript Example Code
```javascript
module.exports = function (context, input, token) {
    context.log('JavaScript manually triggered function called with token:', token);
    context.done();
};
```
#### TypeScript Example Code
```typescript
export function run(context: any, input: any, token: any) {
    context.log(`TypeScript manually triggered function called with token: ${token}`);
    context.done();
};
```
#### Supported types

Token will be input in the form of a *string*