@dapp.schedule(schedule="*/10 * * * * *", arg_name="myTimer", run_on_startup=True)
@dapp.dapr_publish_output(arg_name="pubEvent", pub_sub_name="pubsub", topic="A")
def $(FUNCTION_NAME_INPUT)(myTimer, pubEvent: func.Out[bytes]) -> None:
    """
    Sample Dapr Publish Output Binding
    See https://aka.ms/azure-function-dapr-publish-output-binding for more information about using this binding
    
    These tasks should be completed prior to running :
         1. Install Dapr
         2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Run the app with below steps
         1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
         2. Function will be invoked by Timer trigger and publish messages to message bus.
    """
    logging.info('Python DaprPublish output binding function processed a request.')
    payload = f"Invoked by Timer trigger: Hello, World! The time is {datetime.datetime.now()}"
    pubEvent.set(json.dumps({"payload": payload}).encode('utf-8'))


# Below Azure function will receive message published on topic A, and it will log the message
@dapp.function_name(name="DaprTopicTriggerFuncApp")
@dapp.dapr_topic_trigger(arg_name="subEvent", pub_sub_name="pubsub", topic="A")
def main(subEvent) -> None:
    logging.info('Python Dapr Topic Trigger function processed a request from the Dapr Runtime.')
    subEvent_json = json.loads(subEvent)
    logging.info("Topic A received a message: " + subEvent_json["data"])