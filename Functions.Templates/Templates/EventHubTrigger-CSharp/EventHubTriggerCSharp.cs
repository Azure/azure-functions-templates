#if (portalTemplates)
#r "Microsoft.Azure.EventHubs"

using System;
using System.Text;
using Microsoft.Azure.EventHubs;

public static async Task Run(EventData[] events, ILogger log)
#endif
#if (vsTemplates)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class EventHubTriggerCSharp
    {
        [FunctionName("EventHubTriggerCSharp")]
        public static async Task Run([EventHubTrigger("eventHubNameValue", Connection = "ConnectionValue")] EventData[] events, ILogger log)
#endif
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {                    
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
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
#if (vsTemplates)
    }
}
#endif