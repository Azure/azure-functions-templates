import azure.functions as func
import logging
import json


def main(subEvent,
         value: func.Out[bytes]) -> None:
    """
    Sample Dapr Service Invocation Trigger
    See https://aka.ms/azure-functions-dapr for more information about using this binding
    
    These tasks should be completed prior to running :
         1. Install Dapr
         2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Run the app with below steps
         1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
         2. Invoke function app: dapr publish --pubsub pubsub --publish-app-id functionapp --topic A --data '{\"value\": { \"orderId\": \"42\" } }'
    """
    logging.info('Python DaprTopic trigger with DaprState output binding function processed a request from the Dapr Runtime.')
    payload_json = json.loads(subEvent)
    value.set(json.dumps(payload_json["data"]))
