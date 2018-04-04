#if (portalTemplates)
using System;
using System.Threading.Tasks;

public static void Run(string myQueueItem, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class ServiceBusQueueTriggerCSharp
    {
        [FunctionName("ServiceBusQueueTriggerCSharp")]
        public static void Run([ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")]string myQueueItem, TraceWriter log)
#endif
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
#if (vsTemplates)
    }
}
#endif