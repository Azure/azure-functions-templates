#if (portalTemplates)
#r "Microsoft.Azure.Documents.Client"
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

public static void Run(IReadOnlyList<Document> input, TraceWriter log)
#endif
#if (vsTemplates)
using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs.Extensions.DocumentDB;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace Company.Function
{
    public static class CosmosDBTriggerCSharp
    {
        [FunctionName("CosmosDBTriggerCSharp")]
        public static void Run([CosmosDBTrigger("DatabaseValue", "CollectionValue", ConnectionStringSetting = "ConnectionValue", LeaseDatabaseName = "LeaseDatabaseValue", LeaseCollectionName = "LeaseCollectionValue")] IReadOnlyList<Document> input, TraceWriter log)
#endif
        {
            if (input != null && input.Count > 0) {
                log.Verbose("Documents modified " + input.Count);
                log.Verbose("First document Id " + input[0].Id);
            }
        }
#if (vsTemplates)
    }
}
#endif