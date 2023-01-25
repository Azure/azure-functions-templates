using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class DurableFunctionsOrchestrationCSharp
    {
        [Function(nameof(DurableFunctionsOrchestrationCSharp))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(DurableFunctionsOrchestrationCSharp));
            logger.LogInformation("Saying hello.");
            var outputs = new List<string>();

            // Replace name and input with values relevant for you Durable Functions Activity
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [Function(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SayHello");
            logger.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [Function("DurableFunctionsOrchestrationCSharp_HttpStart")]
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
            // See https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-api
            return durableContext.CreateCheckStatusResponse(req, instanceId);
        }
    }
}