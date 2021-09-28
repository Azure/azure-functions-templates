using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class ServiceBusQueueTriggerCSharp
    {
        private readonly ILogger<ServiceBusQueueTriggerCSharp> _logger;

        public ServiceBusQueueTriggerCSharp(ILogger<ServiceBusQueueTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("ServiceBusQueueTriggerCSharp")]
        public void Run([ServiceBusTrigger("QueueNameValue", Connection = "ConnectionValue")]string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
