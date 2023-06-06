
@app.service_bus_queue_trigger(arg_name="azservicebus", queue_name="$(SERVICEBUS_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azservicebus: func.ServiceBusMessage):
    logging.info('Python ServiceBus Queue trigger processed a message: %s',
                azservicebus.get_body().decode('utf-8'))
