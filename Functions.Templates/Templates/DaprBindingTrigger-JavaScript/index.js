/**
 * Sample Dapr Binding Trigger
 * See https://aka.ms/azure-functions-dapr for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Install Dapr
 *      2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
 * Run the app with below steps
 *      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
 *      2. Function will be invoked by Dapr cron binding trigger and publish messages to message bus.
 * @param context The Azure Function runtime context
 */
module.exports = async function (context) {
    context.log("JavaScript DaprBinding trigger with DaprPublish output binding function processed a request.");

    const message = "Invoked by Dapr cron binding trigger: " + `Hello, World! The time is ${new Date().toISOString()}`;
    
    context.bindings.pubEvent = {
        payload: message
    };
};