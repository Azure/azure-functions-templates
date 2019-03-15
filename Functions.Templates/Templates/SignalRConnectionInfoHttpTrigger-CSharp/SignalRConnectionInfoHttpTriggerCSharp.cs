#if (portalTemplates)
#r "Microsoft.Azure.WebJobs.Extensions.SignalRService"
#endif
#if (vsTemplates)
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
#endif
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

#if (portalTemplates)
public static SignalRConnectionInfo Run(HttpRequest req, SignalRConnectionInfo connectionInfo)
{
    return connectionInfo;
}
#endif
#if (vsTemplates)
namespace Company.Function
{
    public static class SignalRConnectionInfoHttpTriggerCSharp
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
#endif