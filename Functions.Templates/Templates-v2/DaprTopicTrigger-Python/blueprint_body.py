@$(BLUEPRINT_FILENAME).dapr_topic_trigger(arg_name="subEvent", pub_sub_name="pubsub", topic="A")
@$(BLUEPRINT_FILENAME).dapr_state_output(arg_name="state", state_store="statestore", key="order")
def $(FUNCTION_NAME_INPUT)(subEvent, state: func.Out[str]) -> None:
    """
    Sample Dapr topic trigger with dapr_state_output
    See https://aka.ms/azure-functions-dapr for more information about using this binding
    
    These tasks should be completed prior to running :
         1. Install Dapr
         2. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Run the app with below steps
         1. Start function app with Dapr: dapr run --app-id functionapp --app-port 3001 --dapr-http-port 3501 -- func host start
         2. Invoke function app: dapr publish --pubsub pubsub --publish-app-id functionapp --topic A --data '{"value": { "orderId": "1234" } }'
    """
    logging.info('Python DaprTopic trigger with DaprState output binding function processed a request from the Dapr Runtime.')
    subEvent_json = json.loads(subEvent)
    state.set(str(subEvent_json["data"]))