// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Company.Function
{
    using System.Text.Json;
    using CloudNative.CloudEvents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprTopicTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
        /// Invoke function app: dapr publish --pubsub messagebus --publish-app-id functionapp --topic A --data 'This is a tes'
        /// </summary>
        /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
        /// <param name="value">Value will be saved against the given key in state store.</param>
        /// <param name="log">Function logger.</param>
        /// <param name="messagebus">Name of the message bus which is defined in components/messagebus.yaml.</param>
        /// <param name="statestore">Name of the state store which is defined in components/statestore.yaml.</param>
        [FunctionName("DaprTopicTriggerCSharp")]
        public static void Run(
            [DaprTopicTrigger("messagebus", Topic = "A")] CloudEvent subEvent,
            [DaprState("statestore", Key = "product")] out object value,
            ILogger log)
        {
            log.LogInformation("C# DaprTopic trigger with DaprState output binding function processed a request from the Dapr Runtime.");

            value = subEvent.Data;
        }
    }
}