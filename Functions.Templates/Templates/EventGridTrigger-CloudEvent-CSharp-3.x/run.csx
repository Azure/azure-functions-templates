#r "Azure.Messaging.EventGrid"

using Azure.Messaging.EventGrid;

public static void Run(CloudEvent cloudEvent, ILogger log)
{
    log.LogInformation("Received event {type} {subject}", cloudEvent.Type, cloudEvent.Subject);
}
