using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class DurableFunctionsOrchestrationCSharp
    {
        [FunctionName("DurableFunctionsOrchestrationCSharp")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace name and input with values relevant for your Durable Functions Activity
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("DurableFunctionsOrchestrationCSharp_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableClientContext durableContext,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("DurableFunctionsOrchestrationCSharp_HttpStart");

            // Function input comes from the request content.
            string instanceId = await durableContext.Client
                .ScheduleNewOrchestrationInstanceAsync(nameof(DurableFunctionsOrchestrationCSharp));

            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return durableContext.CreateCheckStatusResponse(req, instanceId);
        }
    }
}