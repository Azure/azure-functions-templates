#### Auth token input binding

This binding gets an AAD token for a given resource and provides it to your code as a string. The resource can be any for which the application has permissions. 

#### Configuring an auth token input binding

The binding itself does not require any AAD permissions, but depending on how the token is used, you may need to request additional permissions. Check the requirements of the resource you intend to access with the token.

The binding supports the following properties:

- `name` (*required*): the variable name used in function code for the auth token.
- `type` (*required*): must be set to `token`.
- `direction` (*required*): must be set to `in`.
- `resource` (*required*): An AAD resource URL for which the token is being requested.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`:  Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.

### Using an auth token input binding from code

The token is always presented to code as a string.

#### Sample: Getting user profile information

Suppose you have the following function.json that defines an HTTP trigger with a token input binding:

```json
{
  "bindings": [
    {
      "name": "req",
      "type": "httpTrigger",
      "direction": "in"
    },
    {
      "type": "token",
      "direction": "in",
      "name": "graphToken",
      "resource": "https://graph.microsoft.com",
      "identity": "userFromRequest"
    },
    {
      "name": "$return",
      "type": "http",
      "direction": "out"
    }
  ],
  "disabled": false
}
```

The following C# sample uses the token to make an HTTP call to the Microsoft Graph and returns the result:

```csharp
using System.Net; 
using System.Net.Http; 
using System.Net.Http.Headers; 

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string graphToken, ILogger log)
{
    HttpClient client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);
    return await client.GetAsync("https://graph.microsoft.com/v1.0/me/");
}
```

The following JS sample uses the token to make an HTTP call to the Microsoft Graph and returns the result. In the `function.json` above, change `$return` to `res` first.

```js
const rp = require('request-promise');

module.exports = function (context, req) {
    let token = "Bearer " + context.bindings.graphToken;

    let options = {
        uri: 'https://graph.microsoft.com/v1.0/me/',
        headers: {
            'Authorization': token
        }
    };
    
    rp(options)
        .then(function(profile) {
            context.res = {
                body: profile
            };
            context.done();
        })
        .catch(function(err) {
            context.res = {
                status: 500,
                body: err
            };
            context.done();
        });
};
```