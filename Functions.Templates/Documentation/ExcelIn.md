#### Settings for MS Graph Excel Binding
This binding can only be used with Excel files that reside in OneDrive.

The settings specify the following properties.

- `name` : The variable name used in function code for the Excel input. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *Excel*.
- `Path` : Path from root OneDrive to Excel workbook (e.g. Documents/test.xlsx)
- `WorksheetName` : Worksheet from which user wishes to get data.
- `TableName` : If specified, data will be retrieved from this table. If not, data will be retrieved from the worksheet itself. 
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
        "post",
        "put"
      ],
      "direction": "in"
    },
    {
      "type": "excel",
      "name": "inputTable",
      "Path": "Workbook.xlsx",
      "WorksheetName": "Sheet1",
      "TableName": "Table1",
      "PrincipalId": "{userId}",
      "direction": "in"
    }
  ],
  "disabled": false
}
```
#### C# Example code
```csharp
// This function receives a user's principal ID via a HTTP Request, then reads their Excel table and prints it out
public static void Run(UserInfo info, TraceWriter log, TableRow[] inputTable)
{
	foreach(var row in inputTable) {
		log.Info($"Recieved input: {row.ID}")
	}
}

public class TableRow {
	public string ID { get; set; }
	public string name { get; set; }
	public string number { get; set; }
}

public class UserInfo
{     
    [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
    public string UserId { get; set; }
}
```

#### C# Supported types

[Input] Excel data can be imported to user code using any of the following types:

* WorkbookTable
* string[][]
* List<POCO>*
* POCO[]*

*Where POCO is a user-specified type whose fields exactly match the headers of your table. 

#### JavaScript Example Code
```javascript
module.exports = function (context, req, inputTable) {

    var multiDimensionalArray = JSON.parse(inputTable);

    context.log(multiDimensionalArray[0][0]);
    context.log("--------------");
    context.log(tableInput)
    
    context.done();
};
```