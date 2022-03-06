#r "Azure.Messaging.EventGrid"

using Azure.Messaging.EventGrid;

public static void Run(EventGridEvent eventGridEvent, ILogger log)
{
    log.LogInformation(eventGridEvent.EventType.ToString());
}
