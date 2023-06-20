# This function an HTTP starter function for Durable Functions.
# Before running this sample, please:
# - create a Durable orchestration function
# - create a Durable activity function (default name is "Hello")
# - add azure-functions-durable to requirements.txt
# - run pip install -r requirements.txt
 
import logging

from azure.functions import HttpRequest, HttpResponse
from azure.durable_functions import DurableOrchestrationClient


async def main(req: HttpRequest, starter: str) -> HttpResponse:
    client = DurableOrchestrationClient(starter)
    instance_id = await client.start_new(req.route_params["functionName"], None, None)

    logging.info(f"Started orchestration with ID = '{instance_id}'.")

    return client.create_check_status_response(req, instance_id)