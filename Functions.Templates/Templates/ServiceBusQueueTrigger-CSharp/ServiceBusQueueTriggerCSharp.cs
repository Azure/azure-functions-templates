#if (portalTemplates)
using System;
using System.Threading.Tasks;

public static void Run(string myQueueItem, ILogger log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ServiceBusQueueTriggerCSharp
    {
        [FunctionName("ServiceBusQueueTriggerCSharp")]
        public static void Run([ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")]string myQueueItem, ILogger log)
#endif
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
#if (vsTemplates)
    }
}
#endif