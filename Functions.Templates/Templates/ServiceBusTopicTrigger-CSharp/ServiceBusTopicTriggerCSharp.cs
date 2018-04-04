#if (portalTemplates)
using System;
using System.Threading.Tasks;

public static void Run(string mySbMsg, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class ServiceBusTopicTriggerCSharp
    {
        [FunctionName("ServiceBusTopicTriggerCSharp")]
        public static void Run([ServiceBusTrigger("TopicNameValue", "SubscriptionNameValue", Connection = "ConnectionValue")]string mySbMsg, TraceWriter log)
#endif
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
#if (vsTemplates)
    }
}
#endif