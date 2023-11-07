using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
#if NetCore
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#elif NetFramework
using Microsoft.Azure.Functions.Worker.Http;
#endif

namespace Company.Function
{
    public class HttpTriggerCSharp
    {
        private readonly ILogger<HttpTriggerCSharp> _logger;

        public HttpTriggerCSharp(ILogger<HttpTriggerCSharp> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerCSharp")]
#if NetCore
        public IActionResult Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult("Welcome to Azure Functions!");
        }
#elif NetFramework
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
#endif
    }
}
