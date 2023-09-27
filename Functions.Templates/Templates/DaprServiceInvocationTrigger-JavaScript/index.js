/**
 * Sample Dapr Service Invocation Trigger
 * See https://aka.ms/azure-functions-dapr for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Install Dapr
 * Run the app with below steps
 *      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
 *      2. Invoke function app by dapr cli: dapr invoke --app-id functionapp --method {yourFunctionName}  --data '{ \"data\": {\"value\": { \"orderId\": \"41\" } } }'
 * @param context The Azure Function runtime context
 */
module.exports = async function (context) {
    context.log("Azure function triggered by Dapr Service Invocation Trigger.");
    context.log(`Dapr service invocation trigger payload: ${JSON.stringify(context.bindings.payload)}`);
};