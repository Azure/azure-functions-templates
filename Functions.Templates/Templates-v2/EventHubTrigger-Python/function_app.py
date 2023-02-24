from azure.functions import FunctionApp, EventHubEvent
import logging

app = FunctionApp()

@app.event_hub_message_trigger(arg_name="azeventhub", event_hub_name="$(EVENTHUB_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azeventhub: EventHubEvent):
    logging.info('Python EventHub trigger processed an event: %s',
                azeventhub.get_body().decode('utf-8'))
