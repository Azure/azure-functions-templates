namespace Company.Function
{
    using CloudNative.CloudEvents;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Dapr;
    using Microsoft.Extensions.Logging;

    public static class DaprTopicTriggerCSharp
    {
        /// <summary>
        /// Visit https://aka.ms/azure-functions-dapr to learn how to use the Dapr extension.
        /// These tasks should be completed prior to running :
        ///   1. Install Dapr
        /// Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
        /// Invoke function app: dapr publish --pubsub pubsub --publish-app-id functionapp --topic A --data '{\"value\": { \"orderId\": \"42\" } }'
        /// </summary>
        /// <param name="subEvent">Cloud event sent by Dapr runtime.</param>
        /// <param name="functionContext">Function context.</param>
        [Function("DaprTopicTriggerCSharp")]
        [DaprStateOutput("statestore", Key = "product")]
        public static object? Run(
            [DaprTopicTrigger("pubsub", Topic = "A")] CloudEvent subEvent,
            FunctionContext functionContext)
        {
            var log = functionContext.GetLogger("DaprTopicTriggerCSharp");
            log.LogInformation("C# DaprTopic trigger with DaprState output binding function processed a request.");

            return subEvent.Data;
        }
    }
}