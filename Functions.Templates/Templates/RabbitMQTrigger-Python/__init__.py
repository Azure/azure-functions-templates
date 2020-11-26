import logging

import azure.functions as func


def main(myQueueItem) -> None:
    logging.info('Python rabbitmq trigger function processed a queue item: %s', myQueueItem)
