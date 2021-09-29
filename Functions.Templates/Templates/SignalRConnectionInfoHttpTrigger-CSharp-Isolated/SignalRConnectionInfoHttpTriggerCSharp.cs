using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class SignalRConnectionInfoHttpTriggerCSharp
    {
        private readonly ILogger<SignalRConnectionInfoHttpTriggerCSharp> _logger;

        public SignalRConnectionInfoHttpTriggerCSharp(FunctionContext context)
        {
            _logger = context.GetLogger("negotiate");
        }

        [Function("negotiate")]
        public HttpResponseData Negotiate(
            [HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "HubValue")] MyConnectionInfo connectionInfo)
        {
            _logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");

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