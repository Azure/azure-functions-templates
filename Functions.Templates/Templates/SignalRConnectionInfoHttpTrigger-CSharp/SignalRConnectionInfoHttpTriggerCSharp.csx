#r "Microsoft.Azure.WebJobs.Extensions.SignalRService"
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

public static SignalRConnectionInfo Run(HttpRequest req, SignalRConnectionInfo connectionInfo)
{
    return connectionInfo;
}
