import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.office365 as office365

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts

app = func.FunctionApp()


@app.function_name(name="$(FUNCTION_NAME_INPUT)")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
    arg_name="emails",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_NEW_EMAIL_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(emails: list[office365.GraphClientReceiveMessage]) -> None:
    """Triggered when a new email arrives in Office 365."""
    logging.info("OnNewEmail trigger received.")

    for email in emails:
        logging.info("Subject: '%s'.", email.subject)
        logging.info("From: '%s'.", email.from_)
        logging.info("Importance: '%s'.", email.importance)
        logging.info("Has attachments: '%s'.", email.has_attachments)

    logging.info("Batch contains '%s' email(s).", len(emails))