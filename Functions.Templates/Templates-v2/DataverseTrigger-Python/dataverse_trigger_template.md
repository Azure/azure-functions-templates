# Azure Functions: Dataverse Trigger in Python

## Dataverse Trigger

The Dataverse trigger starts one function when a new row is added.
This template includes only `dataverse_trigger_on_row_changed_poll` by default.
It uses the SDK `Item` binding and logs only the row count.
Dataverse row fields are dynamic; access them through each row's
`dynamic_properties` dictionary without logging their contents.

The template uses `GetOnNewItems_V2`, the legacy **When a row is added (Admin Only)**
operation marked **deprecated** in the
[Dataverse connector reference](https://learn.microsoft.com/connectors/commondataservice/).
The function suffix `_on_row_changed_poll` and the
`OnDataverseRowChangedTriggerConfigName` setting name stay unchanged for
compatibility, but this template receives new rows only.

Polling receives events from a Connector Namespace without a public callback.

## Using the Template

The following example uses the
[Python programming model V2](https://aka.ms/pythonprogrammingmodel) to receive
new rows through polling:

```python
import logging
import azure.functions as func

# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.commondataservice as dataverse

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts
# SubscribeWebhookTrigger - list[dataverse.RowChangeNotification]
# BusinessEventsTrigger - list[dataverse.ActionEventNotification]

app = func.FunctionApp()

# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
    arg_name="rows",
    connection="ConnectorNamespace",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="%OnDataverseRowChangedTriggerConfigName%",
)
def dataverse_trigger_on_row_changed_poll(rows: list[dataverse.Item]) -> None:
    """Receive new rows without logging their contents."""
    logging.info("Received Dataverse new rows (%s rows)", len(rows))
```

To run the code generated through the command palette, note the following:

- The function application is named `app`, and its entry point is `function_app.py`.
	The function name input sets the complete handler name; its default is
	`dataverse_trigger_on_row_changed_poll`. Choose a name that is unique in this app
	and no more than 128 characters long. No suffix is added.
	The registered name matches the Python function name and ends in `_poll`.
- Add these packages to `requirements.txt` and install them in your virtual environment:

	```text
	azure-functions>=2.4.0b1 ; python_version >= '3.13'
	azure-functions>=1.27.0b1 ; python_version < '3.13'
	azurefunctions-extensions-connectors
	```

	The connector extension provides the SDK binding. Verify that the package
	version you install exports `commondataservice.Item` before
	running this template.
- For Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
	and deployed app settings.

For resource setup and configuration, see the [connector documentation](https://aka.ms/connectors).
For local testing and deployment, see the [connector trigger documentation](https://aka.ms/connectortrigger).

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [Dataverse polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/dataverseApp/polling)
for other events and the [Dataverse webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/dataverseApp/webhook)
for the Functions webhook delivery model.

> [!CAUTION]
> Keep payloads out of logs. Never commit tokens or private local settings.

## Programming Model V2

The Python V2 model defines triggers and bindings with decorators and supports
multiple functions in one file. You can also organize functions in blueprints
and register them with `app.register_functions()`.

To learn more, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide)
and [Python blueprints](https://aka.ms/azure-functions-python-blueprints).