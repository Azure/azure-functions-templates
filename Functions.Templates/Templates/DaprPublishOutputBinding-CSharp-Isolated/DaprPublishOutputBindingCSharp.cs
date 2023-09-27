namespace Company.Function
{
    using CloudNative.CloudEvents;
    using Microsoft.Azure.Functions.Extensions.Dapr.Core;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Dapr;
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
        /// <param name="functionContext">Function context.</param>
        [Function("DaprPublishOutputBindingCSharp")]
        [DaprPublishOutput(PubSubName = "pubsub", Topic = "A")]
        public static DaprPubSubEvent Run([TimerTrigger("*/10 * * * * *")] object myTimer,
                                          FunctionContext functionContext)
        {
            var log = functionContext.GetLogger("DaprPublishOutputCSharp");
            log.LogInformation("C# DaprPublish output binding function processed a request.");

            return new DaprPubSubEvent("Invoked by Timer trigger: " + $"Hello, World! The time is {System.DateTime.Now}");
        }
    }

    // Below Azure function will receive message published on topic A, and it will log the message
    public static class DaprTopicTriggerFuncApp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// This function will get invoked when a message is published on topic A
        /// </summary>
        /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
        /// <param name="functionContext">Function context.</param>
        [Function("DaprTopicTriggerFuncApp")]
        public static void Run(
            [DaprTopicTrigger("pubsub", Topic = "A")] CloudEvent subEvent,
            FunctionContext functionContext)
        {
            var log = functionContext.GetLogger("DaprTopicTriggerFuncApp");
            log.LogInformation("C# Dapr Topic Trigger function processed a request from the Dapr Runtime.");
            log.LogInformation($"Topic A received a message: {subEvent.Data}.");
        }
    }
}