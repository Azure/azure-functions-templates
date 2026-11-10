

import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.onedriveforbusiness as onedrive

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts


@$(BLUEPRINT_FILENAME).function_name(name="$(FUNCTION_NAME_INPUT)")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@$(BLUEPRINT_FILENAME).connector_trigger(
    arg_name="files",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_NEW_FILE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(files: list[onedrive.BlobMetadata]) -> None:
    """Triggered when a new file is created in OneDrive for Business."""
    logging.info("OnOneDriveNewFile trigger received.")

    for file in files:
        logging.info("Name: '%s'.", file.name)
        logging.info("Path: '%s'.", file.path)
        logging.info("Size: '%s'.", file.size)