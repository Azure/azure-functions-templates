import logging

import azure.functions as func


def main(message: func.QueueMessage) -> None:
    # Log the received message as plaintext.

    # The `message` argument
    message = message.get_body().decode("utf-8")

    logging.info("Python queue trigger function processed a queue item: " + message)
