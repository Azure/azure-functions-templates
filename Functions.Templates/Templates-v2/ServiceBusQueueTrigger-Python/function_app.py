import azure.functions as func
import logging

app = func.FunctionApp()

@app.service_bus_queue_trigger(arg_name="azservicebus", queue_name="$(SERVICEBUS_NAME_INPUT)",
                               connection="$(CONNECTION_STRING_INPUT)") 
def $(FUNCTION_NAME_INPUT)(azservicebus: func.ServiceBusMessage):
    logging.info('Python ServiceBus Queue trigger processed a message: %s',
                azservicebus.get_body().decode('utf-8'))


# This example uses SDK types to directly access the underlying ServiceBusReceivedMessage object provided by the Service Bus trigger.
# To use, uncomment the section below and add azurefunctions-extensions-bindings-servicebus to your requirements.txt file
# import azurefunctions.extensions.bindings.servicebus as servicebus
# @app.service_bus_queue_trigger(arg_name="receivedmessage",
#                                queue_name="QUEUE_NAME",
#                                connection="SERVICEBUS_CONNECTION")
# def servicebus_queue_trigger(receivedmessage: servicebus.ServiceBusReceivedMessage):
#     logging.info("Python ServiceBus queue trigger processed message.")
#     logging.info("Receiving: %s\n"
#                  "Body: %s\n",
#                  receivedmessage,
#                  receivedmessage.body)