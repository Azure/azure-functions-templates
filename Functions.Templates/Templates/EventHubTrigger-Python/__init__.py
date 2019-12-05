import logging

import azure.functions as func


def main(events: func.EventHubEvent):
    for event in events:
        logging.info('Python EventHub trigger processed an event: %s',
                        event.get_body().decode('utf-8'))
