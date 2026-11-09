

import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.office365 as office365

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.office365.
#
# OnFlaggedEmailV4 - list[office365.GraphClientReceiveMessage]
# OnNewMentionMeEmailV3 - list[office365.GraphClientReceiveMessage]
# SharedMailboxOnNewEmailV2 - list[office365.GraphClientReceiveMessage]
# CalendarGetOnNewItemsV3 - list[office365.GraphCalendarEventClientReceive]
# CalendarGetOnUpdatedItemsV3 - list[office365.GraphCalendarEventClientReceive]
# OnUpcomingEventsV3 - list[office365.GraphCalendarEventClientReceive]
# CalendarGetOnChangedItemsV3 - list[office365.GraphCalendarEventClientWithActionType]
# Configure a trigger config for the selected operation and its required parameters.


@$(BLUEPRINT_FILENAME).function_name(name="$(FUNCTION_NAME_INPUT)_on_new_email_poll")
@$(BLUEPRINT_FILENAME).connector_trigger(
    arg_name="emails",
    connection="$(CONNECTION_INPUT)",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="$(ON_NEW_EMAIL_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)_on_new_email_poll(emails: list[office365.GraphClientReceiveMessage]) -> None:
    """Triggered when a new email arrives in Office 365."""
    logging.info("OnNewEmail trigger received.")

    for email in emails:
        logging.info("Subject: '%s'.", email.subject)
        logging.info("From: '%s'.", email.from_)
        logging.info("Importance: '%s'.", email.importance)
        logging.info("Has attachments: '%s'.", email.has_attachments)

    logging.info("Batch contains '%s' email(s).", len(emails))