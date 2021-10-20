using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class QueueTriggerCSharp
    {
        private readonly ILogger _logger;

        public QueueTriggerCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QueueTriggerCSharp>();
        }

        [Function("QueueTriggerCSharp")]
        public void Run([QueueTrigger("PathValue", Connection = "ConnectionValue")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
