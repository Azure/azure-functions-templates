using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;
public class EventHubTriggerCSharp
{
    private readonly ILogger<EventHubTriggerCSharp> _logger;

    public EventHubTriggerCSharp(ILogger<EventHubTriggerCSharp> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EventHubTriggerCSharp))]
    public void Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            _logger.LogInformation("Event Body: {body}", @event.Body);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }
    }
}