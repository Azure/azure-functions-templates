// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Company.Function
{
    using CloudNative.CloudEvents;
    using Microsoft.Azure.Functions.Extensions.Dapr.Core;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprBindingTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
        /// Function will be invoked by Dapr cron binding trigger and publish messages to message bus.
        /// </summary>
        /// <param name="functionContext">Function context.</param>
        [Function("DaprBindingTriggerCSharp")]
        [DaprPublishOutput(PubSubName = "messagebus", Topic = "A")]
        public static DaprPubSubEvent Run([DaprBindingTrigger(BindingName = "scheduled")] string data,
                                          FunctionContext functionContext)
        {
            var log = functionContext.GetLogger("DaprBindingTriggerCSharp");
            log.LogInformation("C# DaprBinding trigger with DaprPublish output binding function processed a request.");

            return new DaprPubSubEvent("Invoked by Dapr cron binding trigger: " + $"Hello, World! The time is {System.DateTime.Now}");
        }
    }

    // If you want to act on the published message on topic A, please uncomment below Azure function to receive message published on topic A

    // public static class DaprTopicTriggerFuncApp
    // {
    //     /// <summary>
    //     /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
    //     /// This function will get invoked when a message is published on topic A
    //     /// </summary>
    //     /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
    //     /// <param name="functionContext">Function context.</param>
    //     [Function("DaprTopicTriggerFuncApp")]
    //     public static void Run(
    //         [DaprTopicTrigger("messagebus", Topic = "A")] CloudEvent subEvent,
    //         FunctionContext functionContext)
    //     {
    //         var log = functionContext.GetLogger("DaprTopicTriggerFuncApp");
    //         log.LogInformation("C# Dapr Topic Trigger function processed a request from the Dapr Runtime.");
    //         log.LogInformation($"Topic A received a message: {subEvent.Data}.");
    //     }
    // }
}