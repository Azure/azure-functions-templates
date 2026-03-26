# Azure Cosmos DB for MongoDB Trigger

The Azure Cosmos DB for MongoDB trigger uses the MongoDB change stream to listen for inserts and updates. The change stream returns data only if it meets the trigger criteria.

## Example

The following example shows a C# function that is invoked when there are inserts or updates in the specified database and collection.

```csharp
[Function("CosmosDBMongoTriggerCSharp")]
public void Run([CosmosDBMongoTrigger(
    databaseName: "MonitoredDatabaseName",
    collectionName: "MonitoredCollectionName",
    ConnectionStringSetting = "MonitoredConnectionStringSetting",
    LeaseDatabaseName = "LeaseDatabaseName",
    LeaseCollectionName = "LeaseCollectionName",
    LeaseConnectionStringSetting = "LeaseConnectionStringSetting")] BsonDocument input)
{
    if (input != null)
    {
        _logger.LogInformation("Document modified: " + input["_id"]);
    }
}
```

## Settings

| Property | Description |
|----------|-------------|
| **DatabaseName** | The name of the Azure Cosmos DB for MongoDB database containing the collection being monitored. |
| **CollectionName** | The name of the collection being monitored. |
| **ConnectionStringSetting** | The name of an app setting that contains the MongoDB connection string used to connect to the Azure Cosmos DB for MongoDB account being monitored. |
| **LeaseDatabaseName** | The name of the database that holds the lease collection used to track change stream state. |
| **LeaseCollectionName** | The name of the collection used to store leases. Defaults to `leases`. |
| **LeaseConnectionStringSetting** | The name of an app setting that contains the MongoDB connection string for the lease cluster. |
