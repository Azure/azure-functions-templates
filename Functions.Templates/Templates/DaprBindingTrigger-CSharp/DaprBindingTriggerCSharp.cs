// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Company.Function
{
    using CloudNative.CloudEvents;
    using Microsoft.Azure.Functions.Extensions.Dapr.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprBindingTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
        /// Function will be invoked by Dapr cron binding trigger and publish messages to message bus.
        /// </summary>
        /// <param name="log">Function logger.</param>
        [FunctionName("DaprBindingTriggerCSharp")]
        public static void Run([DaprBindingTrigger(BindingName = "scheduled")] string data,
                               [DaprPublish(PubSubName = "messagebus", Topic = "A")] out DaprPubSubEvent pubEvent,
                               ILogger log)
        {
            log.LogInformation("C# DaprBinding trigger with DaprPublish output binding function processed a request.");

            pubEvent = new DaprPubSubEvent("Invoked by Dapr cron binding trigger: " + $"Hello, World! The time is {System.DateTime.Now}");
        }
    }

    public static class DaprTopicTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
        /// Invoke function app: dapr invoke --app-id functionapp --method {functionName} my-secret
        /// </summary>
        /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
        /// <param name="value">Value will be saved against the given key in state store.</param>
        /// <param name="log">Function logger.</param>
        [FunctionName("DaprTopicTriggerCSharp")]
        public static void Run(
            [DaprTopicTrigger("messagebus", Topic = "A")] CloudEvent subEvent,
            [DaprState("statestore", Key = "product")] out object value,
            ILogger log)
        {
            log.LogInformation("C# DaprTopic trigger with DaprState output binding function processed a request from the Dapr Runtime.");
            log.LogInformation($"Topic A received a message: {subEvent.Data}.");

            value = subEvent.Data;
        }
    }
}