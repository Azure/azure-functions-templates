#if (portalTemplates)
using System;
using System.Threading.Tasks;

public static void Run(string mySbMsg, ILogger log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ServiceBusTopicTriggerCSharp
    {
        [FunctionName("ServiceBusTopicTriggerCSharp")]
        public static void Run([ServiceBusTrigger("TopicNameValue", "SubscriptionNameValue", Connection = "ConnectionValue")]string mySbMsg, ILogger log)
#endif
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
#if (vsTemplates)
    }
}
#endif