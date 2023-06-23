import logging

from azure.functions import ServiceBusMessage


def main(msg: ServiceBusMessage):
    logging.info('Python ServiceBus queue trigger processed message: %s',
                 msg.get_body().decode('utf-8'))
