#### Settings for an Azure MySql output binding

The settings for a MySql output binding specifies the following properties:

- `type` : Must be set to *mysql*.
- `name` : The name of the variable that represents the entity in function code.
- `direction` : Must be set to *out*.
- `commandText` : The name of the table being written to by the binding.
- `connectionStringSetting` : The name of an app setting that contains the connection string for the database to which data is being written. This isn't the actual connection string and must instead resolve to an environment variable.

#### MySql output C# code example

This C# code example upserts Products to Products table. 

```csharp
public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "addproduct")]
        HttpRequest req, ILogger log,
        [MySql("Products", "MySqlConnectionString")]
        out Product[] output)
    {
        output = new Product[]
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Sample Product",
                    Cost = 100
                },
            };

        return new CreatedResult($"/api/addproduct", output);
    }
```