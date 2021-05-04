from typing import List
import logging

import azure.functions as func

IoTHubEvent = func.EventHubEvent

def main(iotHubMessages: List[IoTHubEvent]):
    for iotHubMessage in iotHubMessages:
        logging.info('Python IoT Hub trigger processed a message: %s',
                        iotHubMessage.get_body().decode('utf-8'))
