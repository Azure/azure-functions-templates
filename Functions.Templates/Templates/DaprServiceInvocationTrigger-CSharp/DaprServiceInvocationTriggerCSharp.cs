namespace Company.Function
{
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprServiceInvocationTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// These tasks should be completed prior to running :
        ///   1. Install Dapr
        ///   2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
        /// Invoke function app: dapr invoke --app-id functionapp --method {functionName} my-secret
        /// </summary>
        /// <param name="payload">Payload of dapr service invocation trigger.</param>
        /// <param name="secrets">Secrets retrieved from secret store.</param>
        /// <param name="log">Function logger.</param>
        [FunctionName("DaprServiceInvocationTriggerCSharp")]
        public static void Run(
            [DaprServiceInvocationTrigger] object payload,
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