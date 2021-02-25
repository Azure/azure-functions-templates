using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class EventHubTriggerCSharp
    {
        [Function("EventHubTriggerCSharp")]
        public static void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] string input, FunctionContext context)
        {
            var logger = context.GetLogger("EventHubsFunction");
            logger.LogInformation($"C# Event Hub trigger function processed a message: {input}");
        }
    }
}
