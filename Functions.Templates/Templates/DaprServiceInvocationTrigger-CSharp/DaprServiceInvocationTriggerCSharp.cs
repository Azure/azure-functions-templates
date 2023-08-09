namespace Company.Function
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprServiceInvocationTriggerCSharp
    {
        // Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        [FunctionName("DaprServiceInvocationTriggerCSharp")]
        public static void Run(
            [DaprServiceInvocationTrigger] JsonElement payload,
            [DaprSecret("kubernetes", "my-secret", Metadata = "metadata.namespace=default")] IDictionary<string, string> secret,
            ILogger log)
        {
            log.LogInformation("C# ServiceInvocation trigger with DaprSecret input binding function processed a request.");

            // print the fetched secret value
            // this is only for demo purpose
            // please do not log any real secret in your production code
            log.LogInformation("Stored secret: Key = {0}, Value = {1}", secret["foo"]);
        }
    }
}