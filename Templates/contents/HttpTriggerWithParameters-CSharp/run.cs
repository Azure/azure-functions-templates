#if (portalTemplates)
using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, string name, TraceWriter log)
#endif
#if (vsTemplates)
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class HttpTriggerWithParametersCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = "HttpTriggerCSharp/name/{name}")]HttpRequestMessage req, string name, TraceWriter log)
#endif
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
#if (vsTemplates)
    }
}
#endif