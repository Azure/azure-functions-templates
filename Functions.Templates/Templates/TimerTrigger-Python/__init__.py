import logging
from datetime import datetime

import azure.functions as func


def main(schedule: func.TimerRequest) -> None:
    # Log the current UTC time according to the schedule set in `function.json`

    # Get the current UTC time
    utc_timestamp = datetime.utcnow().isoformat()
    logging.info("Python TimerTrigger ran at " + utc_timestamp + " UTC")
