# Azure Functions: Teams Trigger in Python

## Teams Trigger

This template includes one polling handler for new Teams channel messages:
`teams_trigger_on_new_channel_message_poll` by default. It receives
`list[teams.ChatMessage]` and logs the decoded item count.

Polling receives events from a Connector Namespace without a public callback.

## Using the Template

This [Python programming model V2](https://aka.ms/pythonprogrammingmodel) example
receives new channel messages through polling:

```python
import logging
import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.teams as teams

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts

app = func.FunctionApp()

@app.function_name(name="teams_trigger_on_new_channel_message_poll")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
		arg_name="messages",
		delivery_mode=func.DeliveryMode.POLL,
		connection="ConnectorNamespace",
		trigger_config_name="%OnNewChannelMessageTriggerConfigName%",
)
def teams_trigger_on_new_channel_message_poll(messages: list[teams.ChatMessage]) -> None:
		"""Receive new channel messages."""
		logging.info("Received OnNewChannelMessage trigger (%s items)", len(messages))
```

- The function application is named `app`, and its entry point is `function_app.py`.
	The function name input sets the complete handler name; its default is
	`teams_trigger_on_new_channel_message_poll`. Choose a name that is unique in this app
	and no more than 128 characters long. No suffix is added.
- Add these packages to your requirements file and install them in your virtual
	environment:

	```text
	azure-functions>=2.4.0b1 ; python_version >= '3.13'
	azure-functions>=1.27.0b1 ; python_version < '3.13'
	azurefunctions-extensions-connectors
	```

- On Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
	and deployed app settings. The main app is `app`; register blueprints with
	`app.register_functions()`.

For resource setup and configuration, see the [connector documentation](https://aka.ms/connectors).
For local testing and deployment, see the [connector trigger documentation](https://aka.ms/connectortrigger).

Delivery can repeat; this handler does not deduplicate. Keep customer payloads,
tokens, and private settings out of logs and source control.

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [Teams polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/teamsApp/polling)
for other events and the [Teams webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/teamsApp/webhook)
for the Functions webhook delivery model.

For the app and blueprint model, see the
[Python developer guide](https://aka.ms/pythondeveloperguide) and
[Python blueprints](https://aka.ms/azure-functions-python-blueprints).