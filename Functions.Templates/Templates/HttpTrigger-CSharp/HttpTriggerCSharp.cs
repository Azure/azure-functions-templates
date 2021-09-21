using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class HttpTriggerCSharp
    {
        private readonly ILogger<HttpTriggerCSharp> log;

        public HttpTriggerCSharp(ILogger<HttpTriggerCSharp> log)
        {
            this.log = log;
        }

        [FunctionName("HttpTriggerCSharp")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.AuthLevelValue, "get", "post", Route = null)] HttpRequest req)
        {
            log.LogInformation("Test1 C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Test1 This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Test1 Hello, {name}. Test1 This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
