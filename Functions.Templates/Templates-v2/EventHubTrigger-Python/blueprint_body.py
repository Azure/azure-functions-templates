
@$(BLUEPRINT_FILENAME).event_hub_message_trigger(arg_name="azeventhub", event_hub_name="$(EVENTHUB_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azeventhub: func.EventHubEvent):
    logging.info('Python EventHub trigger processed an event: %s',
                azeventhub.get_body().decode('utf-8'))
