import datetime
import logging
import json
import azure.functions as func

def main(myTimer: func.TimerRequest,
         pubEvent: func.Out[bytes]) -> None:
    """
    Sample Dapr Publish Output Binding
    See https://aka.ms/azure-function-dapr-publish-output-binding for more information about using this binding
    
    These tasks should be completed prior to running :
         1. Install Dapr
    Run the app with below steps
         1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
         2. Function will be invoked by Timer trigger and publish messages to message bus.
    """
    logging.info('Python  DaprPublish output binding function processed a request.')
    payload = f"Invoked by Timer trigger: Hello, World! The time is {datetime.datetime.now()}"
    pubEvent.set(json.dumps({"payload": payload}).encode('utf-8'))