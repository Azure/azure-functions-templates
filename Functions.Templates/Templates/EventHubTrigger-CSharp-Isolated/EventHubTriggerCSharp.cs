using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.EventHubs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class EventHubTriggerCSharp
    {
        [FunctionName("EventHubTriggerCSharp")]
        public static void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] string input, FunctionContext context)
        {
            var logger = context.Logger;
            logger.LogInformation($"C# Event Hub trigger function processed a message: {input}");
        }
    }
}
