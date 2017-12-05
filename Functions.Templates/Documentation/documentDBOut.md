#### Settings for Azure Cosmos DB output binding

The following settings can be specified in either the portal or by using the `function.json` in the Advanced Editor with the corresponding property names:

- **Document parameter name** or `name` : Variable name used in function code for the new output document.
- **Output type** or `type` : must be set to *"documentdb"*. This parameter is automatically set if using the Azure Portal.
- **Database name** or `databaseName` : The database containing the collection where the new document will be created.
- **Collection Name** or `collectionName` : The collection where the new document will be created.
- **If true, creates the Azure Cosmos DB database and collection** or `createIfNotExists` : This is a boolean value to indicate whether the collection will be created if it does not exist. The default is *false*. The reason for this is new collections are created with reserved throughput, which has pricing implications. For more details, please visit the [pricing page](https://azure.microsoft.com/en-us/pricing/details/cosmos-db/).
- **Azure Cosmos DB account connection** or `connection` : This string must be an **Application Setting** set to the endpoint for your Azure Cosmos DB account. If you choose your account from the **Integrate** tab, a new App setting will be created for you with a name that takes the following form, `yourAccount_COSMOSDG`. If you need to manually create the App setting, the actual connection string must take the following form, `AccountEndpoint=<Endpoint for your account>;AccountKey=<Your primary access key>;`. 
- **Binding direction** or `direction` : must be set to *"out"*. This is automatically set if using the Azure Portal.

#### Azure Cosmos DB output code example for a JavaScript queue trigger

```javascript
module.exports = function (context, input) {
    
    context.bindings.document = {
        text : "I'm running in a JavaScript function! Data: '" + input + "'"
    }   
    
    context.done();
};
```

The output document:

```json
{
    "text": "I'm running in a JavaScript function! Data: 'example queue data'",
    "id": "01a817fe-f582-4839-b30c-fb32574ff13f"
}
```

#### Azure Cosmos DB output code example for a C# queue trigger

```csharp
public static void Run(string myQueueItem, out object document, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    
    document = new {
        text = $"I'm running in a C# function! {myQueueItem}"
    };
}
```

#### Azure Cosmos DB output code example setting file name

If you want to set the name of the document in the function, just set the `id` value.  For example, if JSON content for an employee was being dropped into the queue similar to the following:

```json
{
    "name" : "John Henry",
    "employeeId" : "123456",
    "address" : "A town nearby"
}
```

You could use the following C# code in a queue trigger function: 
	
```csharp
#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static void Run(string myQueueItem, out object employeeDocument, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    
    dynamic employee = JObject.Parse(myQueueItem);
    
    employeeDocument = new {
        id = employee.name + "-" + employee.employeeId,
        name = employee.name,
        employeeId = employee.employeeId,
        address = employee.address
    };
}
```

Example output:

```json
{
    "id": "John Henry-123456",
    "name": "John Henry",
    "employeeId": "123456",
    "address": "A town nearby"
}
```