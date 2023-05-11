
@app.queue_message_trigger(arg_name="azqueue", queue_name="$(EVENTHUB_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azqueue: func.QueueMessage):
    logging.info('Python EventHub trigger processed a message: %s',
                azqueue.get_body().decode('utf-8'))
