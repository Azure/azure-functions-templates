using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SignalRConnectionInfoHttpTriggerCSharp
    {
        [Function("negotiate")]
        public static HttpResponseData Negotiate(
            [HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "HubValue")] MyConnectionInfo connectionInfo, FunctionContext context)
        {
            var logger = context.GetLogger("negotiate");
            logger.LogInformation($"SignalR Connection URL = '{connectionInfo.Url}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"Connection URL = '{connectionInfo.Url}'");
            
            return response;
        }
    }

    public class MyConnectionInfo
    {
        public string Url { get; set; }

        public string AccessToken { get; set; }
    }
}