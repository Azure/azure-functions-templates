#if (portalTemplates)
using System;

public static void Run(string myEventHubMessage, ILogger log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class EventHubTriggerCSharp
    {
        [FunctionName("EventHubTriggerCSharp")]
        public static void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")]string myEventHubMessage, ILogger log)
#endif
        {
            log.LogInformation($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
#if (vsTemplates)
    }
}
#endif