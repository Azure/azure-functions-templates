#if (portalTemplates)
using System;

public static void Run(string myEventHubMessage, TraceWriter log)
#endif
#if (vsTemplates)
using IoTHubTrigger = Microsoft.Azure.WebJobs.ServiceBus.EventHubTriggerAttribute;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;

namespace Company.Function
{
    public static class IoTHubTriggerCSharp
    {
        [FunctionName("IoTHubTriggerCSharp")]
        public static void Run([IoTHubTrigger("PathValue", Connection = "ConnectionValue")]EventData message, TraceWriter log)
#endif
        {
#if (portalTemplates)
            log.Info($"C# IoT Hub trigger function processed a message: {myEventHubMessage}");
#endif
#if (vsTemplates)
            log.Info($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
#endif
        }
#if (vsTemplates)
    }
}
#endif