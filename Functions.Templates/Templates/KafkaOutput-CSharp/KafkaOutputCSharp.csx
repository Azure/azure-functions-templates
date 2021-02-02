#r "Microsoft.Azure.WebJobs.Extensions.Kafka"
#r "Newtonsoft.Json"

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static IActionResult Run(
    HttpRequest req,
    out string eventData,
    ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string message = req.Query["message"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    message = message ?? data?.message;

    string responseMessage = string.IsNullOrEmpty(message)
        ? "This HTTP triggered function executed successfully. Pass a message in the query string"
        : $"Message {message} sent to the broker. This HTTP triggered function executed successfully.";
    eventData = $"Received message: {message}";

    return new OkObjectResult(responseMessage);
}
