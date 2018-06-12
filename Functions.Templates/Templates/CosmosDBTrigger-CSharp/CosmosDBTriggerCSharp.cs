#if (portalTemplates)
#r "Microsoft.Azure.DocumentDB.Core"
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

public static void Run(IReadOnlyList<Document> input, ILogger log)
#endif
#if (vsTemplates)
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class CosmosDBTriggerCSharp
    {
        [FunctionName("CosmosDBTriggerCSharp")]
        public static void Run([CosmosDBTrigger(
            databaseName: "DatabaseValue",
            collectionName: "CollectionValue",
            ConnectionStringSetting = "ConnectionValue",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
#endif
        {
            if (input != null && input.Count > 0) {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
#if (vsTemplates)
    }
}
#endif