using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class CosmosDBTriggerCSharp
    {
        private readonly ILogger<CosmosDBTriggerCSharp> _logger;

        public CosmosDBTriggerCSharp(ILogger<CosmosDBTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("CosmosDBTriggerCSharp")]
        public void Run([CosmosDBTrigger(
            databaseName: "DatabaseValue",
            collectionName: "CollectionValue",
            ConnectionStringSetting = "ConnectionValue",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified " + input.Count);
                _logger.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
