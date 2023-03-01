
@$(BLUEPRINT_FILENAME).event_hub_message_trigger(arg_name="azeventhub", event_hub_name="$(EVENTHUB_NAME)",
                               connection="$(EVENTHUB_CONNECTION_STRING)") 
def $(FUNCTION_NAME_INPUT)(azeventhub: EventHubEvent):
    logging.info('Python EventHub trigger processed an event: %s',
                azeventhub.get_body().decode('utf-8'))
