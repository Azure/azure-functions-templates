using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class ServiceBusTopicTriggerCSharp
    {
        [Function("ServiceBusTopicTriggerCSharp")]
        public static void Run([ServiceBusTrigger("TopicNameValue", "SubscriptionNameValue", Connection = "ConnectionValue")] string mySbMsg, FunctionContext context)
        {
            var logger = context.GetLogger("ServiceBusTopicTriggerCSharp");
            logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
