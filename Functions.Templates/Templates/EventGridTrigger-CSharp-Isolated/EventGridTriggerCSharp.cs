// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class EventGridTriggerCSharp
    {
        private readonly ILogger<EventGridTriggerCSharp> _logger;

        public EventGridTriggerCSharp(FunctionContext context)
        {
            _logger = context.GetLogger("EventGridTriggerCSharp");
        }

        [Function("EventGridTriggerCSharp")]
        public void Run([EventGridTrigger] MyEvent input)
        {
            _logger.LogInformation(input.Data.ToString());
        }
    }

    public class MyEvent
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Subject { get; set; }

        public string EventType { get; set; }

        public DateTime EventTime { get; set; }

        public object Data { get; set; }
    }
}
