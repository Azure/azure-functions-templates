# Azure Functions: Dataverse Trigger in Python

## Dataverse Trigger

The Dataverse trigger starts one function when a row is added, modified, or deleted.
This template includes only `dataverse_trigger_on_row_changed_poll` by default.
It uses the SDK `RowChangeNotification` binding and logs only the row count.
Dataverse row fields are dynamic; access them through each row's
`additional_properties` dictionary without logging their contents.

Polling receives events from a Connector Namespace without a public callback.

## Using the Template

The following example uses the
[Python programming model V2](https://aka.ms/pythonprogrammingmodel) to receive
row changes through polling:

```python
import logging
import azure.functions as func

# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.commondataservice as dataverse

app = func.FunctionApp()

@app.connector_trigger(
  arg_name="rows",
    connection="ConnectorNamespace",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="%OnDataverseRowChangedTriggerConfigName%",
)
def dataverse_trigger_on_row_changed_poll(rows: list[dataverse.RowChangeNotification]) -> None:
  """Receive row changes without logging their contents."""
  logging.info("Received OnDataverseRowChanged trigger (%s items)", len(rows))
```

To run the code generated through the command palette, note the following:

- The function application is named `app`, and its entry point is `function_app.py`.
  The function name prefix defaults to `dataverse_trigger`. Use a unique
  prefix of 1 to 100 letters, digits, or underscores, starting with a letter.
  The registered name matches the Python function name and ends in `_poll`.
- Add these packages to `requirements.txt` and install them in your virtual environment:

  ```text
  azure-functions>=2.4.0b1 ; python_version >= '3.13'
  azure-functions>=1.27.0b1 ; python_version < '3.13'
  azurefunctions-extensions-connectors>=1.0.0b4
  ```

  The connector extension provides the SDK binding. Verify that the package
  version you install exports `commondataservice.RowChangeNotification` before
  running this template.
- For Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
  and deployed app settings.
- Create a Connector Namespace and authorize an OAuth Dataverse connection in the
  [Connector Namespaces portal](https://connectors.azure.com/). Use the organization's
  HTTPS URL, such as `https://contoso.crm.dynamics.com`, and an OAuth account with
  permission to register organization-scoped event subscriptions. Row-change configs
  use `SubscribeWebhookTrigger` with `organization`, logical `entityname` (for example,
  `account`, not entity-set name `accounts`), `message=7`, and `scope=4`.
  The template generates code, not infrastructure or trigger configs.
- For polling, create a Poll config for `SubscribeWebhookTrigger` and set
  `OnDataverseRowChangedTriggerConfigName` to its name. The trigger config input
  accepts `%OnDataverseRowChangedTriggerConfigName%` or a literal name. Configure
  `ConnectorNamespace__pollingEndpoint` with the service-derived base endpoint and
  grant the host identity access to receive and acknowledge events.
- For local polling tests, stop the deployed app or use separate configs so hosts
  do not compete for events.
- Verify Connected consent, Enabled configs, and successful source registration.
  An Enabled config alone does not prove source registration succeeded. Change a
  test row and check for `Received OnDataverseRowChanged trigger`.

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