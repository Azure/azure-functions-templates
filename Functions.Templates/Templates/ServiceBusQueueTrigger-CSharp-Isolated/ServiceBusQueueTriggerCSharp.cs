using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class ServiceBusQueueTriggerCSharp(ILogger<ServiceBusQueueTriggerCSharp> logger)
{
    [Function(nameof(ServiceBusQueueTriggerCSharp))]
    public async Task Run(
        [ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}