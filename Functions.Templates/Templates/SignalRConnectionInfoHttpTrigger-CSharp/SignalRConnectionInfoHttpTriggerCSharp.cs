using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Company.Function
{
    public static class SignalRConnectionInfoHttpTriggerCSharp
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "HubValue")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}