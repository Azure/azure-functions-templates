using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class HttpTriggerCSharp(ILogger<HttpTriggerCSharp> logger)
{
    [Function("HttpTriggerCSharp")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}