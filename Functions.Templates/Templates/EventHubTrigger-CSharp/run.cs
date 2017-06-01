#if (portalTemplates)
using System;

public static void Run(string myEventHubMessage, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace Company.Function
{
    public static class EventHubTriggerCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static void Run([EventHubTrigger("PathValue", Connection = "ConnectionValue")]string myEventHubMessage, TraceWriter log)
#endif
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
#if (vsTemplates)
    }
}
#endif