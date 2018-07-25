#### Excel table input binding

This binding reads the contents of an Excel table stored in OneDrive.

#### Configuring an Excel table input binding

This binding requires the following AAD permissions:
- `Resource`: Microsoft Graph
- `Permission`: Read user files

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the Excel table.
- `type`: *(required)* must be set to `excel`.
- `direction`: *(required)* must be set to `in`.
- `path`: *(required)* the path in OneDrive to the Excel workbook.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.
- `worksheetName`: The worksheet in which the table is found.
- `tableName`: The name of the table. If not specified, the contents of the worksheet will be used.

#### Using an Excel table input binding from code

The binding exposes the following types to .NET functions:
- string[][]
- Microsoft.Graph.WorkbookTable
- Custom object types (using structural model binding)

#### Sample: Reading an Excel table

Suppose you have the following function.json that defines an HTTP trigger with an Excel input binding:

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
      "type": "excel",
      "direction": "in",
      "name": "excelTableData",
      "path": "{query.workbook}",
      "identity": "UserFromRequest",
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

The following C# sample adds reads the contents of the specified table and returns them to the user:

```csharp
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives; 

public static IActionResult Run(HttpRequest req, string[][] excelTableData, ILogger log)
{
    return new OkObjectResult(excelTableData);
}
```

The following JS sample adds reads the contents of the specified table and returns them to the user. In the `function.json` above, change `$return` to `res` first.

```js
module.exports = function (context, req) {
    context.res = {
        body: context.bindings.excelTableData
    };
    context.done();
};
```