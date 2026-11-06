

import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.sharepointonline as sharepoint


@app.function_name(name="$(FUNCTION_NAME_INPUT)_on_new_file_poll")
@app.connector_trigger(
    arg_name="files",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_NEW_FILE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)_on_new_file_poll(files: list[sharepoint.Item]) -> None:
    """Triggered when a new file is created in SharePoint Online."""
    logging.info("OnSharepointNewFile trigger received.")

    for file in files:
        columns = file.dynamic_properties or {}
        logging.info("Name: '%s'.", columns.get("{FilenameWithExtension}"))
        logging.info("Path: '%s'.", columns.get("{Path}"))
        logging.info("Item ID: '%s'.", columns.get("ID"))