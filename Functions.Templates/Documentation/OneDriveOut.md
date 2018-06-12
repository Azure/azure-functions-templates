#### OneDrive file output binding

This binding modifies the contents of a file stored in OneDrive.

#### Configuring a OneDrive file output binding

This binding requires the following AAD permissions:
- `Resource`: Microsoft Graph
- `Permission`: Have full access to user files

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for file.
- `type`: *(required)* must be set to `onedrive`.
- `direction`: *(required)* must be set to `out`.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.
- `path`: *(required)* the path in OneDrive to the file.

### Using a OneDrive file output binding from code

The binding exposes the following types to .NET functions:
- byte[]
- Stream
- string
- Microsoft.Graph.DriveItem

#### Sample: Writing to a file in OneDrive

Suppose you have the following function.json that defines an HTTP trigger with a OneDrive output binding:

```json
{
  "bindings": [
    {
      "authLevel": "anonymous",
      "name": "req",
      "type": "httpTrigger",
      "direction": "in"
    },
    {
      "name": "myOneDriveFile",
      "type": "onedrive",
      "direction": "out",
      "path": "FunctionsTest.txt",
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

The following C# sample gets text from the query string and writes it to a text file (FunctionsTest.txt as defined in the config above) at the root of the caller's OneDrive:

```csharp
using System.Net;
using System.Text;

public static async Task Run(HttpRequest req, ILogger log, Stream myOneDriveFile)
{
    string data = req.Query
        .FirstOrDefault(q => string.Compare(q.Key, "text", true) == 0)
        .Value;
    await myOneDriveFile.WriteAsync(Encoding.UTF8.GetBytes(data), 0, data.Length);
    return;
}
```
The following JS sample gets text from the query string and writes it to a text file (FunctionsTest.txt as defined in the config above) at the root of the caller's OneDrive. In the `function.json` above, change `$return` to `res` first.

```js
module.exports = function (context, req) {
    context.bindings.myOneDriveFile = req.query.text;
    context.done();
};
```