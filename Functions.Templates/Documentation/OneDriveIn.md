#### OneDrive file input binding

This binding reads the contents of a file stored in OneDrive.

#### Configuring a OneDrive file input binding

This binding requires the following AAD permissions:
- `Resource`: Microsoft Graph
- `Permission`: Read user files

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the file.
- `type`: *(required)* must be set to `onedrive`.
- `direction`: *(required)* must be set to `in`.
- `path`: *(required)* the path in OneDrive to the file.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.

### Using a OneDrive file input binding from code

The binding exposes the following types to .NET functions:
- byte[]
- Stream
- string
- Microsoft.Graph.DriveItem

#### Sample: Reading a file from OneDrive

Suppose you have the following function.json that defines an HTTP trigger with a OneDrive input binding:

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
      "direction": "in",
      "path": "{query.filename}",
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

The following C# sample reads the file specified in the query string and logs its length:

```csharp
using System.Net;

public static void Run(HttpRequestMessage req, Stream myOneDriveFile, ILogger log)
{
    log.LogInformation(myOneDriveFile.Length.ToString());
}
```

The following JS sample reads the file specified in the query string and returns its length. In the `function.json` above, change `$return` to `res` first.

```js
module.exports = function (context, req) {
    context.res = {
        body: context.bindings.myOneDriveFile.length
    };
    context.done();
};
```
