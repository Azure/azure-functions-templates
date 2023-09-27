namespace Company.Function
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Extensions.Dapr.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System.IO;
    using System.Threading.Tasks;

    public static class DaprServiceInvocationTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// These tasks should be completed prior to running :
        ///   1. Install Dapr
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
        /// Invoke function app by dapr cli: dapr invoke --app-id functionapp --method {yourFunctionName}  --data '{ \"data\": {\"value\": { \"orderId\": \"41\" } } }'
        /// Invoke function app by http trigger: 
        /// curl 'http://localhost:7071/api/invoke/functionapp/{yourFunctionName}' `
        /// --header 'Content-Type: application/json' `
        /// --data '{
        ///     "data": {
        ///         "value": {
        ///             "orderId": "41"
        ///         }
        ///     }
        /// }'
        /// <param name="payload">Payload of dapr service invocation trigger.</param>
        /// <param name="log">Function logger.</param>
        /// </summary>
        [FunctionName("DaprServiceInvocationTriggerCSharp")]
        public static void Run(
            [DaprServiceInvocationTrigger] string payload,
            ILogger log)
        {
            log.LogInformation("Azure function triggered by Dapr Service Invocation Trigger.");
            log.LogInformation($"Dapr service invocation trigger payload: {payload}");
        }
    }

    public static class InvokeOutputBinding
    {
        /// <summary>
        /// Sample to use a Dapr Invoke Output Binding to perform a Dapr Server Invocation operation hosted in another Darp'd app.
        /// Here this function acts like a proxy
        /// </summary>
        [FunctionName("InvokeOutputBinding")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "invoke/{appId}/{methodName}")] HttpRequest req,
            [DaprInvoke(AppId = "{appId}", MethodName = "{methodName}", HttpVerb = "post")] IAsyncCollector<InvokeMethodParameters> output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var outputContent = new InvokeMethodParameters
            {
                Body = requestBody
            };

            await output.AddAsync(outputContent);

            return new OkObjectResult("Successfully performed service invocation using Dapr invoke output binding.");
        }
    }
}