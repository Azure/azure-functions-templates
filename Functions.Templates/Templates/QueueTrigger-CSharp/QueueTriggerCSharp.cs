using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class QueueTriggerCSharp
    {
        [FunctionName("QueueTriggerCSharp")]
        public static void Run([QueueTrigger("PathValue", Connection = "ConnectionValue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
