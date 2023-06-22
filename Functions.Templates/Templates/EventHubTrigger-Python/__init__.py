import logging

from azure.functions import EventHubEvent
from typing import List


def main(events: List[EventHubEvent]):
    for event in events:
        logging.info('Python EventHub trigger processed an event: %s',
                      event.get_body().decode('utf-8'))
