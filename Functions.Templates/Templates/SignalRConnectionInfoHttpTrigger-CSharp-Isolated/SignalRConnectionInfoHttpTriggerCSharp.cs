using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class SignalRConnectionInfoHttpTriggerCSharp
{
    private readonly ILogger _logger;

    public SignalRConnectionInfoHttpTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("negotiate");
    }

    [Function("negotiate")]
    public HttpResponseData Negotiate(
        [HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "HubValue")] MyConnectionInfo connectionInfo)
    {
        _logger.LogInformation("SignalR Connection URL = '{url}'", connectionInfo.Url);

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