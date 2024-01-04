#r "Azure.Messaging.EventHubs"
#r "System.Memory.Data"

using System;
using System.Text;
using Azure.Messaging.EventHubs;

public static async Task Run(EventData[] events, ILogger log)
{
    var exceptions = new List<Exception>();

    foreach (EventData eventData in events)
    {
        try
        {
            // Replace these two lines with your processing logic.
            log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
            await Task.Yield();
        }
        catch (Exception e)
        {
            // We need to keep processing the rest of the batch - capture this exception and continue.
            // Also, consider capturing details of the message that failed processing so it can be processed again later.
            exceptions.Add(e);
        }
    }

    // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

    if (exceptions.Count > 1)
        throw new AggregateException(exceptions);

    if (exceptions.Count == 1)
        throw exceptions.Single();
}
