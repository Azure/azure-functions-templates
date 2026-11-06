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

app = func.FunctionApp()

@app.function_name(name="teams_trigger_on_new_channel_message_poll")
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
  The function name input sets the handler's prefix; its default is `teams_trigger`.
  Choose a prefix that is unique in this app.
- Add these packages to your requirements file and install them in your virtual
  environment:

  ```text
  azure-functions>=2.4.0b1 ; python_version >= '3.13'
  azure-functions>=1.27.0b1 ; python_version < '3.13'
  azurefunctions-extensions-connectors>=1.0.0b4
  ```

- On Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
  and deployed app settings. The main app is `app`; register blueprints with
  `app.register_functions()`.
- Create a Connector Namespace and authorize a Teams OAuth connection in the
  [Connector Namespaces portal](https://connectors.azure.com/). Select the team /
  Microsoft 365 group **object ID** and **URL-decoded channel ID**, not display names.
  The template creates code, not resources, consent, or source subscriptions.
- For polling, set `ConnectorNamespace__pollingEndpoint` to the namespace polling
  base endpoint and grant the host identity receive/acknowledge access. Configure
  identity credentials for that prefix; `ConnectorNamespace` is not the Teams
  connection name. Set `OnNewChannelMessageTriggerConfigName` to the matching Poll
  config name for `OnNewChannelMessage`. The trigger config input accepts
  `%OnNewChannelMessageTriggerConfigName%` or a literal config name.

Check Connected consent, Enabled configs, **and successful source registration**;
Enabled alone does not prove registration. Test new root channel messages, not
replies. Confirm
successful invocations and actual item counts. Stop competing deployed/local hosts
or use separate polling configs. Delivery can repeat; these handlers do not
deduplicate. Keep customer payloads, tokens, and private settings out
of logs and source control.

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [Teams polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/teamsApp/polling)
for other events and the [Teams webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/teamsApp/webhook)
for the Functions webhook delivery model.

For the app and blueprint model, see the
[Python developer guide](https://aka.ms/pythondeveloperguide) and
[Python blueprints](https://aka.ms/azure-functions-python-blueprints).