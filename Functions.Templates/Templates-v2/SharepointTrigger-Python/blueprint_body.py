

import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.sharepointonline as sharepoint

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts
# GetOnNewFileItems - list[sharepoint.Item]
# GetOnUpdatedFileItems - list[sharepoint.Item]
# GetOnNewItems - list[sharepoint.Item]
# GetOnUpdatedItems - list[sharepoint.Item]
# GetOnChangedItems - list[sharepoint.Item]
# GetOnNewItemsFromForm - list[sharepoint.Item]
# GetOnRecurrenceDigest - list[sharepoint.Item]
# GetOnUpdatedFileClassifiedTimes - list[sharepoint.Item]
# GetOnDeletedItems - list[sharepoint.DeletedItem]
# GetOnDeletedFileItems - list[sharepoint.DeletedItem]


@$(BLUEPRINT_FILENAME).function_name(name="$(FUNCTION_NAME_INPUT)")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@$(BLUEPRINT_FILENAME).connector_trigger(
    arg_name="files",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_NEW_FILE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(files: list[sharepoint.Item]) -> None:
    """Triggered when a new file is created in SharePoint Online."""
    logging.info("OnSharepointNewFile trigger received.")

    for file in files:
        columns = file.dynamic_properties or {}
        logging.info("Name: '%s'.", columns.get("{FilenameWithExtension}"))
        logging.info("Path: '%s'.", columns.get("{Path}"))
        logging.info("Item ID: '%s'.", columns.get("ID"))