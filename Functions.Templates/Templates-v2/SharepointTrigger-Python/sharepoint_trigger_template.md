# Azure Functions: SharePoint Trigger in Python

## SharePoint Trigger

The SharePoint trigger starts one function when a file is created in SharePoint
Online. This template includes only `sharepoint_trigger_on_new_file_poll` by default.

Polling receives events from a Connector Namespace without a public callback.

## Using the Template

The following example uses SDK types with the
[Python programming model V2](https://aka.ms/pythonprogrammingmodel) to process
new files through polling:

```python
import logging
import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.sharepointonline as sharepoint

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.sharepointonline.
#
# GetOnUpdatedFileItems - list[sharepoint.Item]
# GetOnNewItems - list[sharepoint.Item]
# GetOnUpdatedItems - list[sharepoint.Item]
# GetOnChangedItems - list[sharepoint.Item]
# GetOnNewItemsFromForm - list[sharepoint.Item]
# GetOnRecurrenceDigest - list[sharepoint.Item]
# GetOnUpdatedFileClassifiedTimes - list[sharepoint.Item]
# GetOnDeletedItems - list[sharepoint.DeletedItem]
# GetOnDeletedFileItems - list[sharepoint.DeletedItem]
# Configure a trigger config for the selected operation and its required parameters.

app = func.FunctionApp()

@app.connector_trigger(
    arg_name="files",
    connection="ConnectorNamespace",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="%OnSharepointNewFileTriggerConfigName%",
)
def sharepoint_trigger_on_new_file_poll(files: list[sharepoint.Item]) -> None:
    for file in files:
        columns = file.dynamic_properties or {}
        logging.info("Name: '%s'.", columns.get("{FilenameWithExtension}"))
```

Read file fields from `dynamic_properties or {}`.

To run the code generated through the command palette, note the following:

- The function application is named `app`, and its entry point is `function_app.py`.
  The function name input sets the complete handler name; its default is
  `sharepoint_trigger_on_new_file_poll`. Choose a name that is unique in this app
  and no more than 128 characters long. No suffix is added.
- Add these packages to `requirements.txt` and install them in your virtual environment:

  ```text
  azure-functions>=2.4.0b1 ; python_version >= '3.13'
  azure-functions>=1.27.0b1 ; python_version < '3.13'
  azurefunctions-extensions-connectors
  ```

- For Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
  and deployed app settings.
- Create a Connector Namespace and authorize a SharePoint Online connection in the
  [Connector Namespaces portal](https://connectors.azure.com/). Choose a site and
  document library that your account can access. Set `dataset` to the site URL
  and `table` to the library name in the trigger config. The template generates
  code, not Azure resources or trigger configs.
- For polling, create a Poll config for `GetOnNewFileItems` and set
  `OnSharepointNewFileTriggerConfigName` to its name. The trigger config input
  accepts `%OnSharepointNewFileTriggerConfigName%` or a literal name.
  Configure `ConnectorNamespace__pollingEndpoint` with the service-derived base
  endpoint and grant the host identity access to receive and acknowledge events.
- For local polling tests, stop the deployed app or use separate configs so the
  hosts do not compete for events.

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [SharePoint polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/sharepointApp/polling)
for other events and the [SharePoint webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/sharepointApp/webhook)
for the Functions webhook delivery model.

> [!CAUTION]
> The handler logs file names, paths, and item details. Use a test library and
> restrict log access. Never commit tokens or private local settings.

## Programming Model V2

The Python V2 model defines triggers and bindings with decorators and supports
multiple functions in one file. You can also organize functions in blueprints
and register them with `app.register_functions()`.

To learn more, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide)
and [Python blueprints](https://aka.ms/azure-functions-python-blueprints).