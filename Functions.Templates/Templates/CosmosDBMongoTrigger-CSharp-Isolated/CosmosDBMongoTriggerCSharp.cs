using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.AzureCosmosDb.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Company.Function;

public class CosmosDBMongoTriggerCSharp
{
    private readonly ILogger<CosmosDBMongoTriggerCSharp> _logger;

    public CosmosDBMongoTriggerCSharp(ILogger<CosmosDBMongoTriggerCSharp> logger)
    {
        _logger = logger;
    }

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
}
