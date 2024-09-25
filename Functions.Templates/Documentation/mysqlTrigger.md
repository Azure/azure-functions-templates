#### Settings for an Azure MySql trigger binding

The settings for a MySql trigger binding specifies the following properties:

- `type` : Must be set to *mysqlTrigger*.
- `name` : The name of the parameter that the trigger binds to.
- `direction` : Must be set to *in*.
- `tableName` : The name of the table being monitored by the trigger.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database containing the table monitored for changes. This isn't the actual connection string and must instead resolve to an environment variable.

#### MySql trigger C# code example

This C# code example monitors the Products table.

```csharp
public static void Run(
            [MySqlTrigger("Products", "SqlConnectionString")]
            IReadOnlyList<MySqlChange<Product>> changes,
            ILogger logger)
        {
            // The output is used to inspect the trigger binding parameter in test methods.
            logger.LogInformation("MySql Changes: " + JsonConvert.SerializeObject(changes));
        }
```
