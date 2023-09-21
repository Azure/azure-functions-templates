@$(BLUEPRINT_FILENAME).schedule(schedule="*/10 * * * * *", arg_name="myTimer", run_on_startup=True)
@$(BLUEPRINT_FILENAME).dapr_publish_output(arg_name="pubEvent", pub_sub_name="pubsub", topic="A")
def $(FUNCTION_NAME_INPUT)(myTimer, pubEvent: func.Out[bytes]) -> None:
    logging.info('Python DaprPublish output binding function processed a request.')
    payload = f"Invoked by Timer trigger: Hello, World! The time is {datetime.datetime.now()}"
    pubEvent.set(json.dumps({"payload": payload}).encode('utf-8'))


# Dapr topic trigger
@$(BLUEPRINT_FILENAME).function_name(name="DaprTopicTriggerFuncApp")
@$(BLUEPRINT_FILENAME).dapr_topic_trigger(arg_name="subEvent", pub_sub_name="pubsub", topic="A")
def main(subEvent) -> None:
    logging.info('Python Dapr Topic Trigger function processed a request from the Dapr Runtime.')
    subEvent_json = json.loads(subEvent)
    logging.info("Topic A received a message: " + subEvent_json["data"])