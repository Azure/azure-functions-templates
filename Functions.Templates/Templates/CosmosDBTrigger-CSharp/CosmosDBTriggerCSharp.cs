#if (portalTemplates)
// DocumentDB output binding has been added to this function with no functional use. 
// This has been done as workaround of a bug that is present in functions runtime. 
// It forces functions runtime to load certain binaries needed for CosmosDBTrigger to work properly.
// The DocumentDB binding just needs to be present in one of the function with in the function app for the workaround to be effective.
#r "Microsoft.Azure.Documents.Client"
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

public static void Run(IReadOnlyList<Document> input, Document outputDocument, TraceWriter log)
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
        public static void Run([CosmosDBTrigger("DatabaseValue", "CollectionValue", ConnectionString = "ConnectionValue", LeaseDatabaseName = "LeaseDatabaseValue", LeaseCollectionName = "LeaseCollectionValue")] IReadOnlyList<Document> input, TraceWriter log)
#endif
        {
            log.Verbose("Documents modified " + input.Count);
            log.Verbose("First document Id " + input[0].Id);
        }
#if (vsTemplates)
    }
}
#endif