# Azure Functions: Outlook Trigger in Python

## Outlook Trigger

The Outlook trigger starts one function when a new Office 365 email arrives.
This template includes only `outlook_trigger_on_new_email_poll` by default.

Polling receives events from a Connector Namespace without a public callback.

## Using the Template

The following example uses SDK types with the
[Python programming model V2](https://aka.ms/pythonprogrammingmodel) to process
new email through polling:

```python
import logging
import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.office365 as office365

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts
# OnNewEmailV3 - list[office365.GraphClientReceiveMessage]
# OnFlaggedEmailV4 - list[office365.GraphClientReceiveMessage]
# OnNewMentionMeEmailV3 - list[office365.GraphClientReceiveMessage]
# SharedMailboxOnNewEmailV2 - list[office365.GraphClientReceiveMessage]
# CalendarGetOnNewItemsV3 - list[office365.GraphCalendarEventClientReceive]
# CalendarGetOnUpdatedItemsV3 - list[office365.GraphCalendarEventClientReceive]
# OnUpcomingEventsV3 - list[office365.GraphCalendarEventClientReceive]
# CalendarGetOnChangedItemsV3 - list[office365.GraphCalendarEventClientWithActionType]

app = func.FunctionApp()

# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
		arg_name="emails",
		connection="ConnectorNamespace",
		delivery_mode=func.DeliveryMode.POLL,
		trigger_config_name="%OnNewEmailTriggerConfigName%",
)
def outlook_trigger_on_new_email_poll(
		emails: list[office365.GraphClientReceiveMessage],
) -> None:
		for email in emails:
				logging.info("Subject: '%s'.", email.subject)
```

To run the code generated through the command palette, note the following:

- The function application is named `app`, and its entry point is `function_app.py`.
	The function name input sets the complete handler name; its default is
	`outlook_trigger_on_new_email_poll`. Choose a name that is unique in this app
	and no more than 128 characters long. No suffix is added.
- Add these packages to `requirements.txt` and install them in your virtual environment:

	```text
	azure-functions>=2.4.0b1 ; python_version >= '3.13'
	azure-functions>=1.27.0b1 ; python_version < '3.13'
	azurefunctions-extensions-connectors
	```

- For Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
	and deployed app settings.

For resource setup and configuration, see the [connector documentation](https://aka.ms/connectors).
For local testing and deployment, see the [connector trigger documentation](https://aka.ms/connectortrigger).

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [Office 365 polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/office365App/polling)
for other events and the [Office 365 webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/office365App/webhook)
for the Functions webhook delivery model.

> [!CAUTION]
> The handler logs email details. Use a test mailbox and restrict log
> access. Never commit tokens or private local settings.

## Programming Model V2

The Python V2 model defines triggers and bindings with decorators and supports
multiple functions in one file. You can also organize functions in blueprints
and register them with `app.register_functions()`.

To learn more, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide)
and [Python blueprints](https://aka.ms/azure-functions-python-blueprints).