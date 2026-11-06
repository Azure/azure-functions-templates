

import logging

import azure.functions as func

# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.commondataservice as dataverse


@$(BLUEPRINT_FILENAME).function_name(name="$(FUNCTION_NAME_INPUT)_on_row_changed_poll")
@$(BLUEPRINT_FILENAME).connector_trigger(
    arg_name="rows",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_ROW_CHANGED_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)_on_row_changed_poll(rows: list[dataverse.RowChangeNotification]) -> None:
    """Receive row changes without logging their contents."""
    logging.info("Received OnDataverseRowChanged trigger (%s items)", len(rows))