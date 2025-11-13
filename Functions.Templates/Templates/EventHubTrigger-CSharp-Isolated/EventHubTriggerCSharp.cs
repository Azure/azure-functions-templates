using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;
public class EventHubTriggerCSharp(ILogger<EventHubTriggerCSharp> logger)
{
    [Function(nameof(EventHubTriggerCSharp))]
    public void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            logger.LogInformation("Event Body: {body}", @event.Body);
            logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }
    }
}