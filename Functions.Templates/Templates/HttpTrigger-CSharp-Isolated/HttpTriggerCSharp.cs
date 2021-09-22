using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class HttpTriggerCSharp
    {
        private readonly ILogger<HttpTriggerCSharp> log;

        public HttpTriggerCSharp(FunctionContext executionContext)
        {
            this.log = executionContext.GetLogger("HttpTriggerCSharp"); ;
        }

        [Function("HttpTriggerCSharp")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post")] HttpRequestData req)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
