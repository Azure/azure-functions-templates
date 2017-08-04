#### Settings for Token Input Binding

The settings specify the following properties:

- `name` : The variable name used in function code to identify the token.
- `type` : Must be set to *token*.
- `direction` : Must be set to *in*. 
- `Resource` : Resource to retrieve an authentication token for. Default is Microsoft Graph.
- `PrincipalId` : Should be set to either an app setting containing the Principal ID/OID to be used to communicate with the specific Resource or an expression to evaluate to a Principal ID/OID
- `idToken` : Should be set to an expression that evaluates to an ID token. Either Principal ID or ID token must be set, but not both.

#### Example function.json #1
```json
{
  "bindings": [
    {
      "type": "httpTrigger",
      "name": "req",
      "authLevel": "anonymous",
      "methods": [
        "get",
        "post"
      ],
      "direction": "in"
    },
    {
      "type": "http",
      "direction": "out",
      "name": "res"
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
using System.Net;

public static HttpResponseMessage Run(UserInfo req, TraceWriter log, string token)
{
    var response = new HttpResponseMessage();
    response.Content = new StringContent("Retrieved token: " + token);
    return response;
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "principalId", NullValueHandling = NullValueHandling.Ignore)]
    public string principalId { get; set; }
}
```

#### Python Example Code
```python
import os
import json

token = open(os.environ['token']).read()
response = open(os.environ['res'], 'w')
response.write("token: "+ token)
response.close()
```

#### Example function.json #2
```json
{
  "bindings": [
    {
      "type": "manualTrigger",
      "direction": "in",
      "name": "input"
    },
    {
      "type": "token",
      "name": "token",
      "PrincipalId": "Identity.alias",
      "direction": "in",
      "Resource": "https://graph.windows.net"
    }
  ],
  "disabled": false
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