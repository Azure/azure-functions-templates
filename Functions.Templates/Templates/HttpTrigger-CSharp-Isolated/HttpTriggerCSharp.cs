using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
#if NET6_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#else
using Microsoft.Azure.Functions.Worker.Http;
#endif

namespace Company.Function
{
    public class HttpTriggerCSharp
    {
        private readonly ILogger<T> _logger;

        public HttpTriggerCSharp(ILogger<T> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerCSharp")]
#if NETCORE
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            return new OkObjectResult("Welcome to Azure Functions!");
        }
#else
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
