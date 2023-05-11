using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Dapr;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class DaprBindingTriggerPublishBindingCSharp
    {
        // Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        [FunctionName("DaprBindingTriggerPublishBindingCSharp")]
        public static void Run(
            [DaprBindingTrigger(BindingName = "%CronBindingName%")] object _args,
            [DaprPublish(PubSubName = "%PubSubName%", Topic = "B")] out JsonElement pubEvent,
            ILogger log)
        {
            log.LogInformation("C# DaprBinding trigger with DaprPublish output binding function processed a request.");

            var newEvent = new
            {
                id = System.Guid.NewGuid().ToString(),
                data = $"Hello, World! The time is {System.DateTime.Now}"
            };

            pubEvent = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(newEvent)).RootElement;
        }
    }
}
