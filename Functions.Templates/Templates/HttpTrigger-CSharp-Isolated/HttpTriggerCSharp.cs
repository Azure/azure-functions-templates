using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class HttpTriggerCSharp
    {
        [Function("HttpTriggerCSharp")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post")] HttpRequestData req,
            FunctionExecutionContext executionContext)
        {
            var logger = executionContext.GetLogger("HttpTriggerCSharp");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = new HttpResponseData(HttpStatusCode.OK);
            var headers = new Dictionary<string, string>();
            headers.Add("Content", "Content - Type: text / html; charset = utf - 8");

            response.Headers = headers;
            response.Body = "Welcome to Azure Functions!";

            return response;
        }
    }
}
