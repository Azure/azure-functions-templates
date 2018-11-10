/*
 * This function is not intended to be invoked directly. Instead it will be
 * triggered by an HTTP starter function.
 * 
 * Before running this sample, please:
 * - create a Durable activity function (default name is "Hello")
 * - create a Durable HTTP starter function
 */

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

public static async Task<List<string>> Run(DurableOrchestrationContext context)
{
    var outputs = new List<string>();

    // Replace "Hello" with the name of your Durable Activity Function.
    outputs.Add(await context.CallActivityAsync<string>("Hello", "Tokyo"));
    outputs.Add(await context.CallActivityAsync<string>("Hello", "Seattle"));
    outputs.Add(await context.CallActivityAsync<string>("Hello", "London"));

    // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
    return outputs;
}