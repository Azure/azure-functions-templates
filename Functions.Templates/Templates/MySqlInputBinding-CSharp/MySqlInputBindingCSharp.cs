using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class MySqlInputBindingCSharp
    {
    [FunctionName("MySqlInputBindingCSharp")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [MySql("SELECT * FROM object",
            "MySqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with MySql Input Binding function processed a request.");

            return new OkObjectResult(result);
        }
    }
}
