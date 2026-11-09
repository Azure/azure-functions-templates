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

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.commondataservice.
#
# Legacy row polling operations:
# GetOnUpdatedItems_V2 - list[dataverse.Item]
# GetOnDeletedItems_V2 - list[dataverse.Item]
#
# Current row-change and action notifications:
# SubscribeWebhookTrigger - list[dataverse.RowChangeNotification]
# BusinessEventsTrigger - list[dataverse.ActionEventNotification]
# An operation name containing Webhook does not select Functions delivery mode.
# Configure a trigger config for the selected operation and its required parameters.

app = func.FunctionApp()

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
  The function name prefix defaults to `dataverse_trigger`. Use a unique
  prefix of 1 to 100 letters, digits, or underscores, starting with a letter.
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
- Create a Connector Namespace and authorize an OAuth Dataverse connection in the
  [Connector Namespaces portal](https://connectors.azure.com/). Use the organization's
  HTTPS URL, such as `https://contoso.crm.dynamics.com`, and an OAuth account with
  the permissions required by the Admin Only trigger. Configure `GetOnNewItems_V2`
  with `dataset=<org HTTPS URL>` and `table=<plural entity-set name>`, for example,
  `dataset=https://contoso.crm.dynamics.com` and `table=accounts`.
  The template generates code, not infrastructure or trigger configs.
- For polling, create a Poll config for `GetOnNewItems_V2` and set
  `OnDataverseRowChangedTriggerConfigName` to its name. The trigger config input
  accepts `%OnDataverseRowChangedTriggerConfigName%` or a literal name. Configure
  `ConnectorNamespace__pollingEndpoint` with the service-derived base endpoint and
  grant the host identity access to receive and acknowledge events.
- For local polling tests, stop the deployed app or use separate configs so hosts
  do not compete for events.
- Verify Connected consent, Enabled configs, and successful source registration.
  An Enabled config alone does not prove source registration succeeded. Add a
  test row and check for `Received Dataverse new rows`.

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