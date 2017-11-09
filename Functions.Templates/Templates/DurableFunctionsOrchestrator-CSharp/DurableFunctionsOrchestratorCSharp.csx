/*
 * Before running this sample, please create a Durable Activity function (default name is "hello")
 */

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

public static async Task<List<string>> Run(DurableOrchestrationContext context)
{
    var outputs = new List<string>();

    // Replace "hello" with the name of your Durable Activity Function.
    outputs.Add(await context.CallActivityAsync<string>("Hello", "Tokyo"));
    outputs.Add(await context.CallActivityAsync<string>("Hello", "Seattle"));
    outputs.Add(await context.CallActivityAsync<string>("Hello", "London"));

    // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
    return outputs;
}