using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Extensions.SignalRService;
using Microsoft.Azure.Functions.Worker.Pipeline;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SignalRConnectionInfoHttpTriggerCSharp
    {
        [FunctionName("negotiate")]
        public static HttpResponseData Negotiate(
            [HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "HubValue")] MyConnectionInfo connectionInfo, FunctionContext context)
        {
            var logger = context.Logger;
            logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");

            var response = new HttpResponseData(HttpStatusCode.OK);
            var headers = new Dictionary<string, string>();
            headers.Add("Content", "Content - Type: text / html; charset = utf - 8");

            response.Headers = headers;
            response.Body = $"Connection URL = '{connectionInfo.Url}'";

            return response;
        }
    }

    public class MyConnectionInfo
    {
        public string Url { get; set; }

        public string AccessToken { get; set; }
    }
}