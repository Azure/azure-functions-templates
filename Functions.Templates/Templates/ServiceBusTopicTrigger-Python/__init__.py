import logging

from azure.functions import ServiceBusMessage


def main(message: ServiceBusMessage):
    # Log the Service Bus Message as plaintext

    message_content_type = message.content_type
    message_body = message.get_body().decode("utf-8")

    logging.info("Python ServiceBus topic trigger processed message.")
    logging.info("Message Content Type: " + message_content_type)
    logging.info("Message Body: " + message_body)
