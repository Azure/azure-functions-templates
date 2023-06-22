import logging

def main(myQueueItem) -> None:
    logging.info('Python rabbitmq trigger function processed a queue item: %s', myQueueItem)
