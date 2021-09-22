// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class EventGridTriggerCSharp
    {
        private readonly ILogger<EventGridTriggerCSharp> log;

        public EventGridTriggerCSharp(ILogger<EventGridTriggerCSharp> log)
        {
            this.log = log;
        }

        [FunctionName("EventGridTriggerCSharp")]
        public void Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}
