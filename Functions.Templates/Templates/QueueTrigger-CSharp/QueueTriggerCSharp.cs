using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class QueueTriggerCSharp
    {
        private readonly ILogger<QueueTriggerCSharp> _logger;

        public QueueTriggerCSharp(ILogger<QueueTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("QueueTriggerCSharp")]
        public void Run([QueueTrigger("PathValue", Connection = "ConnectionValue")]string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
