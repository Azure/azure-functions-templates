

import logging

import azure.functions as func

# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.commondataservice as dataverse

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts
# SubscribeWebhookTrigger - list[dataverse.RowChangeNotification]
# BusinessEventsTrigger - list[dataverse.ActionEventNotification]


@app.function_name(name="$(FUNCTION_NAME_INPUT)")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
    arg_name="rows",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_ROW_CHANGED_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(rows: list[dataverse.Item]) -> None:
    """Receive new rows without logging their contents."""
    logging.info("Received Dataverse new rows (%s rows)", len(rows))