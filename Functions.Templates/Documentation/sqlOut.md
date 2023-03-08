#### Settings for a Azure SQL output binding

See [Output Binding Overview](https://github.com/Azure/azure-functions-sql-extension/blob/main/docs/BindingsOverview.md#output-binding) for general information about the Azure SQL output binding.

The settings for a SQL output binding specifies the following properties:

- `type` : Must be set to *sql*.
- `name` : The name of the variable that represents the entity in function code.
- `direction` : Must be set to *out*.
- `commandText` : The name of the table being written to by the binding.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database to which data is being written. This isn't the actual connection string and must instead resolve to an environment variable. Optional keywords in the connection string value are [available to refine SQL bindings connectivity](https://aka.ms/sqlbindings#sql-connection-string).

#### SQL output C# code example

This C# code example upserts employees to Employees table.

```csharp
public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "addemployees")]
        HttpRequest req, ILogger log,
        [Sql("dbo.Employees", "SqlConnectionString")]
        out Employee[] output)
    {
        output = new Employee[]
            {
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Hello",
                    LastName = "World",
                    Company = "Microsoft",
                    Team = "Functions"
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Hi",
                    LastName = "SQLupdate",
                    Company = "Microsoft",
                    Team = "Functions"
                },
            };

        return new CreatedResult($"/api/addemployees", output);
    }
```

#### SQL output JavaScript example

```JavaScript
 module.exports = async function (context, req) {
        const employees = [
            {
                EmployeeId : 1,
                FirstName : "Hello",
                LastName : "World",
                Company : "Microsoft",
                Team : "Functions"
            },
            {
                EmployeeId : 2,
                FirstName : "Hi",
                LastName : "SQLupdate",
                Company : "Microsoft",
                Team : "Functions"
            }
        ];
        context.bindings.employee = employees;

        return {
            status: 201,
            body: employees
        };
    }
```