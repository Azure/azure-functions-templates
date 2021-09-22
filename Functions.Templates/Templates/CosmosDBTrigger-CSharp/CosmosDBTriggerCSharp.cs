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
        private readonly ILogger<CosmosDBTriggerCSharp> log;

        public CosmosDBTriggerCSharp(ILogger<CosmosDBTriggerCSharp> log)
        {
            this.log = log;
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
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
