using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Dapr;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class DaprTopicTriggerStateBindingCSharp
    {
        // Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        [FunctionName("DaprTopicTriggerStateBindingCSharp")]
        public static void Run(
            [DaprTopicTrigger("%PubSubName%", Topic = "A")] JsonElement subEvent,
            [DaprState("%StateStoreName%", Key = "product")] out string value,
            ILogger log)
        {
            log.LogInformation("C# DaprTopic trigger with DaprState binding function processed a request.");

            if (subEvent.TryGetProperty("data", out JsonElement data))
            {
                value = data.ToString();
            }

            throw new System.Exception("data property not found");
        }
    }
}
