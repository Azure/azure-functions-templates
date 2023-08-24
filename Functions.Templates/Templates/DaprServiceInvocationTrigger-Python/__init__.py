import logging
import json


def main(payload, secret) -> None:
    """
    Sample Dapr Binding Trigger
    See https://aka.ms/azure-functions-dapr for more information about using this binding
    
    These tasks should be completed prior to running :
         1. Install Dapr
         2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Run the app with below steps
         1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 --resources-path .\components\ -- func host start
         2. Invoke function app: dapr invoke --app-id functionapp --method {functionName} --data '{}'
    """
    logging.info(
        'Python ServiceInvocation trigger with DaprSecret input binding function processed a request.')
    secret_dict = json.loads(secret)
    
    """
    print the fetched secret value
    this is only for demo purpose
    please do not log any real secret in your production code
    """
    for key in secret_dict:
        logging.info("Stored secret: Key = " + key +
                     ', Value = ' + secret_dict[key])