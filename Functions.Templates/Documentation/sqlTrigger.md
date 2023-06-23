#### Settings for an Azure SQL trigger binding

See [Trigger Binding Overview](https://github.com/Azure/azure-functions-sql-extension/blob/main/docs/BindingsOverview.md#trigger-binding) for general information about the Azure SQL trigger binding.

The settings for a SQL trigger binding specifies the following properties:

- `type` : Must be set to *sqlTrigger*.
- `name` : The name of the parameter that the trigger binds to.
- `direction` : Must be set to *in*.
- `tableName` : The name of the table being monitored by the trigger.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database containing the table monitored for changes. This isn't the actual connection string and must instead resolve to an environment variable. Optional keywords in the connection string value are [available to refine SQL bindings connectivity](https://aka.ms/sqlbindings#sql-connection-string).

#### SQL trigger C# code example

This C# code example monitors the Employees table. Please refer to our [TriggerBinding C# Samples](https://github.com/Azure/azure-functions-sql-extension/tree/main/samples/samples-csharp/TriggerBindingSamples) for more examples.

```csharp
public static void Run(
            [SqlTrigger("[dbo].[Employees]", "SqlConnectionString")]
            IReadOnlyList<SqlChange<Employees>> changes,
            ILogger logger)
        {
            // The output is used to inspect the trigger binding parameter in test methods.
            logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));
        }
```

#### SQL trigger JavaScript example

You can browse here for more [JavaScript Samples](https://github.com/Azure/azure-functions-sql-extension/tree/main/samples/samples-js).

```JavaScript
 module.exports = async function (context, changes) {
    context.log(`SQL Changes: ${JSON.stringify(changes)}`)
}
```