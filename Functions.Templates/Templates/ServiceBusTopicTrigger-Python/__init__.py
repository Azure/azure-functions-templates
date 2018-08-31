import logging

import azure.functions as func


def main(msg: func.ServiceBusMessage):
    logging.info('Python ServiceBus topic trigger processed message: %s',
                 msg.get_body().decode('utf-8'))
