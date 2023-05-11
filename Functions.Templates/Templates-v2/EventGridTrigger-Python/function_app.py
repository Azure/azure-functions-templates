from azure.functions import FunctionApp, EventGridEvent
import logging
import json

app = FunctionApp()

@app.event_grid_trigger(arg_name="azeventgrid")
def $(FUNCTION_NAME_INPUT)(azeventgrid: EventGridEvent):
    result = json.dumps({
        'id': azeventgrid.id,
        'data': azeventgrid.get_json(),
        'topic': azeventgrid.topic,
        'subject': azeventgrid.subject,
        'event_type': azeventgrid.event_type,
    })
    logging.info('Python EventGrid trigger processed an event: %s', result)
