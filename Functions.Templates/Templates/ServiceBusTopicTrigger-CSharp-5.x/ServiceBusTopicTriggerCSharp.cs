using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class ServiceBusTopicTriggerCSharp
    {
        private readonly ILogger<ServiceBusTopicTriggerCSharp> _logger;

        public ServiceBusTopicTriggerCSharp(ILogger<ServiceBusTopicTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("ServiceBusTopicTriggerCSharp")]
        public void Run([ServiceBusTrigger("TopicNameValue", "SubscriptionNameValue", Connection = "ConnectionValue")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
