using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class KafkaOutputCSharp
    {
        private readonly ILogger<KafkaOutputCSharp> _logger;

        public KafkaOutputCSharp(ILogger<KafkaOutputCSharp> log)
        {
            _logger = log;
        }
        // KafkaOutputBinding sample
        // This KafkaOutput binding will create a topic "topic" on the LocalBroker if it doesn't exists.
        // Call this function then the KafkaTrigger will be trigged.
        [FunctionName("KafkaOutputCSharp")]
        public IActionResult Output(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Kafka("BrokerList",
                   "topic",
                   Username = "$ConnectionString",
                   Password = "%KafkaPassword%",
                   Protocol = BrokerProtocol.SaslSsl,
                   AuthenticationMode = BrokerAuthenticationMode.Plain
            )] out string eventData)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string message = req.Query["message"];

            string responseMessage = string.IsNullOrEmpty(message)
                ? "This HTTP triggered function executed successfully. Pass a message in the query string"
                : $"Message {message} sent to the broker. This HTTP triggered function executed successfully.";
            eventData = $"Received message: {message}";

            return new OkObjectResult(responseMessage);
        }
    }
}
