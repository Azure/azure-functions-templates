
@app.service_bus_topic_trigger(arg_name="azservicebus", topic_name="$(SERVICEBUS_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azservicebus: func.ServiceBusMessage):
    logging.info('Python ServiceBus trigger processed a message: %s',
                azservicebus.get_body().decode('utf-8'))
