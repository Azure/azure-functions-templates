using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class IotHubTriggerCSharp
    {
        private static HttpClient client = new HttpClient();
        private readonly ILogger<IotHubTriggerCSharp> _logger;

        public IotHubTriggerCSharp(ILogger<IotHubTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("IotHubTriggerCSharp")]
        public void Run([IoTHubTrigger("PathValue", Connection = "ConnectionValue")]EventData message)
        {
            _logger.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}