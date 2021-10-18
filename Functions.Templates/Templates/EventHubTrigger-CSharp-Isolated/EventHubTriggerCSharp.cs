using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class EventHubTriggerCSharp
    {
        private readonly ILogger _logger;

        public EventHubTriggerCSharp(FunctionContext context)
        {
            _logger = context.GetLogger<EventHubTriggerCSharp>();
        }

        [Function("EventHubTriggerCSharp")]
        public void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] string[] input)
        {
            _logger.LogInformation($"First Event Hubs triggered message: {input[0]}");
        }
    }
}
