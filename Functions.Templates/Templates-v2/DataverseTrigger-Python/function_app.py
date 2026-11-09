import logging

import azure.functions as func

# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.commondataservice as dataverse

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.commondataservice.
#
# Legacy row polling operations:
# GetOnUpdatedItems_V2 - list[dataverse.Item]
# GetOnDeletedItems_V2 - list[dataverse.Item]
#
# Current row-change and action notifications:
# SubscribeWebhookTrigger - list[dataverse.RowChangeNotification]
# BusinessEventsTrigger - list[dataverse.ActionEventNotification]
# An operation name containing Webhook does not select Functions delivery mode.
# Configure a trigger config for the selected operation and its required parameters.

app = func.FunctionApp()


@app.function_name(name="$(FUNCTION_NAME_INPUT)_on_row_changed_poll")
@app.connector_trigger(
    arg_name="rows",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_ROW_CHANGED_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)_on_row_changed_poll(rows: list[dataverse.Item]) -> None:
    """Receive new rows without logging their contents."""
    logging.info("Received Dataverse new rows (%s rows)", len(rows))