#r "Azure.Messaging"

using Azure.Messaging;

public static void Run(CloudEvent cloudEvent, ILogger log)
{
    log.LogInformation("Event received {type} {subject}: {data}", cloudEvent.Type, cloudEvent.Subject, cloudEvent.Data.ToString());
}
