// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;

namespace Company.Function
{
    public static class EventGridCloudEventTriggerCSharp
    {
        [FunctionName("EventGridCloudEventTriggerCSharp")]
        public static void Run([EventGridTrigger]CloudEvent cloudEvent, ILogger log)
        {
            log.LogInformation("Event received {type} {subject}", cloudEvent.Type, cloudEvent.Subject);
        }
    }
}
