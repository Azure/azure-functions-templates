#### Outlook message output binding
Sends a mail message through Outlook.

#### Configuring an Outlook message output binding

This binding requires the following AAD permissions:
- `Resource`: Microsoft Graph
- `Permission`: Send mail as user

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the mail message.
- `type`: *(required)* must be set to `outlook`.
- `direction`: *(required)* must be set to `out`.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.

### Using an Outlook message output binding from code

The binding exposes the following types to .NET functions:
- Microsoft.Graph.Message
- Newtonsoft.Json.Linq.JObject
- string
- Custom object types (using structural model binding)

#### Sample: Sending an email through Outlook

Suppose you have the following function.json that defines an HTTP trigger with an Outlook message output binding:

```json
{
  "bindings": [
    {
      "name": "req",
      "type": "httpTrigger",
      "direction": "in"
    },
    {
      "name": "message",
      "type": "outlook",
      "direction": "out",
      "identity": "userFromRequest"
    }
  ],
  "disabled": false
}
```

The following C# sample sends a mail from the caller to a recipient specified in the query string:

```csharp
using System.Net;

public static void Run(HttpRequest req, out Message message, ILogger log)
{ 
    string emailAddress = req.Query["to"];
    message = new Message(){
        subject = "Greetings",
        body = "Sent from Azure Functions",
        recipient = new Recipient() {
            address = emailAddress
        }
    };
}

public class Message {
    public String subject {get; set;}
    public String body {get; set;}
    public Recipient recipient {get; set;}
}

public class Recipient {
    public String address {get; set;}
    public String name {get; set;}
}
```

The following JS sample sends a mail from the caller to a recipient specified in the query string:

```js
module.exports = function (context, req) {
    context.bindings.message = {
        subject: "Greetings",
        body: "Sent from Azure Functions with JavaScript",
        recipient: {
            address: req.query.to 
        } 
    };
    context.done();
};
```