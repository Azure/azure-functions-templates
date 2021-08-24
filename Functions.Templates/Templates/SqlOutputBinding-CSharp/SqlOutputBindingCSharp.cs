using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SqlOutputBindingCSharp
    {
        // Visit https://github.com/Azure/azure-functions-sql-extension#Output-Binding-Tutorial to learn how to use this output binding
        // and https://github.com/Azure/azure-functions-sql-extension#Output-Binding for more details
        [FunctionName("SqlOutputBinding")]
         public static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Sql("table", ConnectionStringSetting = "SqlConnectionString")] out Object output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            throw new NotImplementedException();
        }
    }
}
