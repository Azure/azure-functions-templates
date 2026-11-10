

import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.teams as teams

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts


@$(BLUEPRINT_FILENAME).function_name(name="$(FUNCTION_NAME_INPUT)")
@$(BLUEPRINT_FILENAME).connector_trigger(
    arg_name="messages",
    delivery_mode=func.DeliveryMode.POLL,
    connection="$(CONNECTION_INPUT)",
    trigger_config_name="$(ON_NEW_CHANNEL_MESSAGE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(messages: list[teams.ChatMessage]) -> None:
    """Receive new channel messages."""
    logging.info("Received OnNewChannelMessage trigger (%s items)", len(messages))