using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class KafkaTriggerCSharp
    {
        private readonly ILogger<KafkaTriggerCSharp> _logger;

        public KafkaTriggerCSharp(ILogger<KafkaTriggerCSharp> log)
        {
            _logger = log;
        }

        // KafkaTrigger sample 
        // Consume the message from "topic" on the LocalBroker.
        // Add `BrokerList` and `KafkaPassword` to the local.settings.json
        // For EventHubs
        // "BrokerList": "{EVENT_HUBS_NAMESPACE}.servicebus.windows.net:9093"
        // "KafkaPassword":"{EVENT_HUBS_CONNECTION_STRING}
        [FunctionName("KafkaTriggerCSharp")]
        public void Run(
            [KafkaTrigger("BrokerList",
                          "topic",
                          Username = "$ConnectionString",
                          Password = "%KafkaPassword%",
                          Protocol = BrokerProtocol.SaslSsl,
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                          ConsumerGroup = "$Default")] KafkaEventData<string>[] events)
        {
            foreach (KafkaEventData<string> eventData in events)
            {
                _logger.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");
            }
        }
    }
}
