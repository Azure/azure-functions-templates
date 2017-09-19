#if (portalTemplates)
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

public static IActionResult Run(HttpRequest req, TraceWriter log)
#endif
#if (vsTemplates)
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Company.Function
{
    public static class HttpTriggerCSharp
    {
        [FunctionName("HttpTriggerCSharp")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
#endif
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (req.Query.TryGetValue("name", out StringValues value))
            {
                return new OkObjectResult($"Hello, {value.First()}");
            }

            return new BadRequestObjectResult("Please pass a name on the query string");
        }
#if (vsTemplates)
    }
}
#endif