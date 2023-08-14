namespace FunctionApp8
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class Function1
    {
        // Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        [FunctionName("Function1")]
        public static void Run(
            [DaprServiceInvocationTrigger] JsonElement payload,
            [DaprSecret("localsecretstore", "my-secret", Metadata = "metadata.namespace=default")] IDictionary<string, string> secrets,
            ILogger log)
        {
            log.LogInformation("C# ServiceInvocation trigger with DaprSecret input binding function processed a request.");

            // print the fetched secret value
            // this is only for demo purpose
            // please do not log any real secret in your production code
            foreach (var kvp in secrets)
            {
                log.LogInformation("Stored secret: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}