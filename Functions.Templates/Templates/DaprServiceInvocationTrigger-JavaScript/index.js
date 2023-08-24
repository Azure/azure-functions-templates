/**
 * Sample Dapr Service Invocation Trigger
 * See https://aka.ms/azure-functions-dapr for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Install Dapr
 *      2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
 * Run the app with below steps
 *      1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
 *      2. Invoke function app: dapr invoke --app-id functionapp --method {functionName} my-secret
 * @param context The Azure Function runtime context
 */
module.exports = async function (context) {
    context.log("JavaScript ServiceInvocation trigger with DaprSecret input binding function processed a request.");

    /*
    print the fetched secret value
    this is only for demo purpose
    please do not log any real secret in your production code
    */
    for (var key in context.bindings.secret) {
        context.log(`Stored secret: Key=${key}, Value=${context.bindings.secret[key]}`);
    }
};