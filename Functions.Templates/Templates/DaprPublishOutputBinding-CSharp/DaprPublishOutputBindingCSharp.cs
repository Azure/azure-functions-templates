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

    public static class DaprPublishOutputBindingCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// These tasks should be completed prior to running :
        ///   1. Install Dapr
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
        /// Function will be invoked by Timer trigger and publish messages to message bus.
        /// </summary>
        /// <param name="log">Function logger.</param>
        [FunctionName("DaprPublishOutputBindingCSharp")]
        //  we can try using Function timer trigger.
        // Rename the template to PublishTemplate
        public static void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer,
                               [DaprPublish(PubSubName = "pubsub", Topic = "A")] out DaprPubSubEvent pubEvent,
                               ILogger log)
        {
            log.LogInformation("C# DaprPublish output binding function processed a request.");

            pubEvent = new DaprPubSubEvent("Invoked by Timer trigger: " + $"Hello, World! The time is {System.DateTime.Now}");
        }
    }

    // Below Azure function will receive message published on topic A, and it will log the message
    public static class DaprTopicTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// This function will get invoked when a message is published on topic A
        /// </summary>
        /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
        /// <param name="log">Function logger.</param>
        [FunctionName("DaprTopicTriggerCSharp")]
        public static void Run(
            [DaprTopicTrigger("pubsub", Topic = "A")] CloudEvent subEvent,
            ILogger log)
        {
            log.LogInformation("C# Dapr Topic Trigger function processed a request from the Dapr Runtime.");
            log.LogInformation($"Topic A received a message: {subEvent.Data}.");
        }
    }
}