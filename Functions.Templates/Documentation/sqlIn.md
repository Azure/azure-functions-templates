#### Settings for a Azure SQL input binding

See [Input Binding Overview](https://github.com/Azure/azure-functions-sql-extension/blob/main/docs/BindingsOverview.md#input-binding) for general information about the Azure SQL Input binding.

The settings for a SQL input binding specifies the following properties:

- `type` : Must be set to *sql*.
- `name` : The name of the variable that represents the query results in function code.
- `direction` : Must be set to *in*.
- `commandText` : The Transact-SQL query command or name of the stored procedure executed by the binding.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database against which the query or stored procedure is being executed. This value isn't the actual connection string and must instead resolve to an environment variable name.  Optional keywords in the connection string value are [available to refine SQL bindings connectivity](https://aka.ms/sqlbindings#sql-connection-string).
- `commandType` : A [CommandType](https://learn.microsoft.com/dotnet/api/system.data.commandtype) value, which is [Text](https://learn.microsoft.com/dotnet/api/system.data.commandtype#fields) for a query and [StoredProcedure](https://learn.microsoft.com/dotnet/api/system.data.commandtype#fields) for a stored procedure.
- `parameters` : *optional*. Zero or more parameter values passed to the command during execution as a single string. Must follow the format `@param1=param1,@param2=param2`. Neither the parameter name nor the parameter value can contain a comma (`,`) or an equals sign (`=`).

#### SQL input C# code example

This C# code example copies a blob whose name is received in a queue message.

```csharp
[FunctionName("GetProducts")]
  public static IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
      HttpRequest req,
      [Sql("select * from Products where Cost = @Cost",
          "SqlConnectionString",
          parameters: "@Cost={cost}")]
      IEnumerable<Product> products)
  {
      return (ActionResult)new OkObjectResult(products);
  }
```

#### SQL input JavaScript example

```JavaScript
 module.exports = async function (context, req, product) {
        return {
            status: 200,
            body: product
        };
}
```