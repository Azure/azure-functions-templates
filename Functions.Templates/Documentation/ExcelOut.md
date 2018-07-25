#### Excel table output binding

This binding modifies the contents of an Excel table stored in OneDrive.

#### Configuring an Excel table output binding

This binding requires the following AAD permissions:
- `Resource`: Microsoft Graph
- `Permission`: Have full access to user files

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the auth token.
- `type`: *(required)* must be set to `excel`.
- `direction`: *(required)* must be set to `out`.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.
- `path`: *(required)* the path in OneDrive to the Excel workbook.
- `worksheetName`: The worksheet in which the table is found.
- `tableName`: The name of the table. If not specified, the contents of the worksheet will be used.
- `updateType`: *(required)* The type of change to make to the table. Can be one of the following values:
   - `update`: Replaces the contents of the table in OneDrive.
   - `append`: Adds the payload to the end of the table in OneDrive by creating new rows.

#### Using an Excel table output binding from code

The binding exposes the following types to .NET functions:
- string[][]
- Newtonsoft.Json.Linq.JObject
- Microsoft.Graph.WorkbookTable
- Custom object types (using structural model binding)

#### Sample: Adding rows to an Excel table

Suppose you have the following function.json that defines an HTTP trigger with an Excel output binding:

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
      "name": "newExcelRow",
      "type": "excel",
      "direction": "out",
      "identity": "userFromRequest",
      "updateType": "append",
      "path": "{query.workbook}",
      "tableName": "{query.table}"
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


The following C# sample adds a new row to the table (assumed to be single-column) based on input from the query string:

```csharp
using System.Net;
using System.Text;

public static async Task Run(HttpRequest req, IAsyncCollector<object> newExcelRow, ILogger log)
{
    string input = req.Query
        .FirstOrDefault(q => string.Compare(q.Key, "text", true) == 0)
        .Value;
    await newExcelRow.AddAsync(new {
        Text = input
        // Add other properties for additional columns here
    });
    return;
}
```

The following JS sample adds a new row to the table (assumed to be single-column) based on input from the query string. In the `function.json` above, change `$return` to `res` first.

```js
module.exports = function (context, req) {
    context.bindings.newExcelRow = {
        text: req.query.text
        // Add other properties for additional columns here
    }
    context.done();
};
```
