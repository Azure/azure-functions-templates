/*
 * Before running this sample, please create a Durable Activity function (default name is "hello")
 */

#if (portalTemplates)
#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

public static async Task<List<string>> Run(DurableOrchestrationContext context)
#endif
#if (vsTemplates)
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Company.Function
{
    public static class DurableFunctionsOrchestratorCSharp
    {
        [FunctionName("DurableFunctionsOrchestratorCSharp")]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger] DurableOrchestrationContext context)
#endif
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }
#if (vsTemplates)
    }
}
#endif