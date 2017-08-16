#if (portalTemplates)
using System;

public static void Run(string myIoTHubMessage, TraceWriter log)
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
        public static void Run([EventHubTrigger("PathValue", Connection = "ConnectionValue")]string myIoTHubMessage, TraceWriter log)
#endif
        {
            log.Info($"C# IoT Hub trigger function processed a message: {myIoTHubMessage}");
        }
#if (vsTemplates)
    }
}
#endif