#### Settings for MS Graph Excel Binding

The settings specify the following properties.

- `name` : The variable name used in function code for the Excel input. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *Excel*.
- `UpdateType` : If set to updated, specified rows will replace existing rows in table. If set to append, output rows will be appended to existing rows in table.
- `Path` : Path from root OneDrive to Excel workbook (e.g. Documents/test.xlsx)
- `WorksheetName` : Worksheet from which user wishes to get data.
- `TableName` : If specified, data will be retrieved from this table. If not, data will be retrieved from the worksheet itself. 
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal id/OID. A new app setting containing this user's Principal id can be created by clicking 'retrieve'.
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.

#### General Information
This binding can only be used with Excel files that reside in OneDrive.

This binding can be used to update existing Excel tables and worksheets. If a worksheet name is provided without a table name, the specified worksheet will be updated. If a worksheet and table name are provided, the specified table will be updated.



#### C# Example code
```csharp
// Updating or appending multiple rows using a POCO[]
public static void Run(TimerInfo timer, TraceWriter log, out TableRow[] table)
{
    table = new TableRow[] {
        new TableRow {
            id = "3",
            name = "testItem",
            number = "15"
        },
	new TableRow {
            id = "4",
            name = "testItem2",
            number = "7"
        }
    };  
}

public class TableRow {
	public string id { get; set; }
	public string name { get; set; }
	public string number { get; set; }
}
```

```csharp
// Update a single column using a JObject
public static void Run(TimerInfo timer, TraceWriter log, out JObject col)
{
    col = new JObject();
    col["column"] = "number";
    col["value"] = "13";  
}
```

#### Supported types

[Output] Excel data can be serialized to any of the following types:

* object[][]
* List<POCO>*
* POCO*
* POCO[]*
* JObject**

*Where POCO is a user-specified type whose fields exactly match the headers of your table. 

**JObjects can be used to update a single column with a specific value by settings the "column" and "value" keys.