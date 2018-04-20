using IoTHubTrigger = Microsoft.Azure.WebJobs.ServiceBus.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Text;
using System.Net.Http;

namespace Company.Function
{
    public static class IotHubTriggerCSharp
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("IotHubTriggerCSharp")]
        public static void Run([IoTHubTrigger("PathValue", Connection = "ConnectionValue")]EventData message, TraceWriter log)
        {
            log.Info($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.GetBytes())}");
        }
    }
}