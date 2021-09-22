using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class CosmosDBTriggerCSharp
    {
        private readonly ILogger<CosmosDBTriggerCSharp> log;

        public CosmosDBTriggerCSharp(FunctionContext context)
        {
            this.log = context.GetLogger("CosmosDBTriggerCSharp");
        }

        [Function("CosmosDBTriggerCSharp")]
        public void Run([CosmosDBTrigger(
            databaseName: "DatabaseValue",
            collectionName: "CollectionValue",
            ConnectionStringSetting = "ConnectionValue",
            LeaseCollectionName = "leases")] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                logger.LogInformation("Documents modified: " + input.Count);
                logger.LogInformation("First document Id: " + input[0].Id);
            }
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
