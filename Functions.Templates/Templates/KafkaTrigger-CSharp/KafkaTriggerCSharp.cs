using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class KafkaTriggerCSharp
    {
        // KafkaTrigger sample 
        // Consume the message from "topic" on the LocalBroker.
        // Add LocalBroker to the local.settings.json e.g. "LocalBroker": "localhost:9092"
        [FunctionName("KafkaTrigger")]
        public static void Run(
            [KafkaTrigger("BrokerList",
                          "topic",
                          Username = "$ConnectionString",
                          Password = "%Password%",
                          Protocol = BrokerProtocol.SaslSsl,
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                          ConsumerGroup = "$Default")] KafkaEventData<string>[] events, ILogger log)
        {
            foreach (KafkaEventData<string> eventData in events)
            {
                log.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");
            }
        }

        // KafkaOutputBinding sample
        // This KafkaOutput binding will create a topic "topic" on the LocalBroker if it doesn't exists.
        // Call this function then the KafkaTrigger will be trigged.
        [FunctionName("KafkaOutput")]
        public static IActionResult Output(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Kafka("BrokerList", 
                   "topic",
                   Username = "$ConnectionString",
                   Password = "%Password%",
                   Protocol = BrokerProtocol.SaslSsl,
                   AuthenticationMode = BrokerAuthenticationMode.Plain
            )] out string eventData,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string message = req.Query["message"];

            string responseMessage = string.IsNullOrEmpty(message)
                ? "This HTTP triggered function executed successfully. Pass a message in the query string"
                : $"Message {message} sent to the broker. This HTTP triggered function executed successfully.";
            eventData = $"Received message: {message}";

            return new OkObjectResult(responseMessage);
        }
    }
}
