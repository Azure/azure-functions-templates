#### Settings for an Azure MySql input binding

The settings for a MySql input binding specifies the following properties:

- `type` : Must be set to *mysql*.
- `name` : The name of the variable that represents the query results in function code.
- `direction` : Must be set to *in*.
- `commandText` : The query command or name of the stored procedure executed by the binding.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database against which the query or stored procedure is being executed. This value isn't the actual connection string and must instead resolve to an environment variable name.
- `commandType` : A [CommandType](https://learn.microsoft.com/dotnet/api/system.data.commandtype) value, which is [Text](https://learn.microsoft.com/dotnet/api/system.data.commandtype#fields) for a query and [StoredProcedure](https://learn.microsoft.com/dotnet/api/system.data.commandtype#fields) for a stored procedure.
- `parameters` : *optional*. Zero or more parameter values passed to the command during execution as a single string. Must follow the format `@param1=param1,@param2=param2`. Neither the parameter name nor the parameter value can contain a comma (`,`) or an equals sign (`=`).

#### MySql input C# code example

This C# code example for getting data from Products table. 

```csharp
[FunctionName("GetProducts")]
  public static IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
      HttpRequest req,
      [MySql("select * from Products where Cost = @Cost",
          "MySqlConnectionString",
          parameters: "@Cost={cost}")]
      IEnumerable<Product> products)
  {
      return (ActionResult)new OkObjectResult(products);
  }
```