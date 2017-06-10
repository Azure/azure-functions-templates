#if (portalTemplates)
using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
#endif
#if (vsTemplates)
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class GenericWebHookCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "github")]HttpRequestMessage req, TraceWriter log)
#endif
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Extract github comment from request body
            string gitHubComment = data?.comment?.body;

            return req.CreateResponse(HttpStatusCode.OK, "From Github:" + gitHubComment);
        }
#if (vsTemplates)
    }
}
#endif