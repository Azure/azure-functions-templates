# Azure Functions: OneDrive Trigger in Python

## OneDrive Trigger

The OneDrive trigger starts one function when a file is created in OneDrive for
Business, not personal OneDrive. This template includes only
`onedrive_trigger_on_new_file_poll` by default. The SDK binding receives file
metadata, not file contents.

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
import azurefunctions.extensions.connectors.onedriveforbusiness as onedrive

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.onedriveforbusiness.
#
# OnUpdatedFilesV2 - list[onedrive.BlobMetadata]
# Configure a trigger config for the selected operation and its required parameters.

app = func.FunctionApp()

@app.connector_trigger(
    arg_name="files",
    connection="ConnectorNamespace",
    delivery_mode=func.DeliveryMode.POLL,
    trigger_config_name="%OnOneDriveNewFileTriggerConfigName%",
)
def onedrive_trigger_on_new_file_poll(files: list[onedrive.BlobMetadata]) -> None:
    for file in files:
        logging.info("Name: '%s'.", file.name)
```

To run the code generated through the command palette, note the following:

- The function application is named `app`, and its entry point is `function_app.py`.
  The function name input sets the handler's prefix; its default is `onedrive_trigger`.
  Choose a prefix that is unique in this app.
- Add these packages to `requirements.txt` and install them in your virtual environment:

  ```text
  azure-functions>=2.4.0b1 ; python_version >= '3.13'
  azure-functions>=1.27.0b1 ; python_version < '3.13'
  azurefunctions-extensions-connectors
  ```

- For Python 3.12 and older, set `PYTHON_ISOLATE_WORKER_DEPENDENCIES=1` in local
  and deployed app settings.
- Create a Connector Namespace and authorize an OAuth OneDrive for Business
  connection in the [Connector Namespaces portal](https://connectors.azure.com/).
  Set `folderId` to the connector folder ID or `root`, not a display name or path.
  The template generates code, not Azure resources or trigger configs.
- For polling, create a Poll config for `OnNewFilesV2` and set
  `OnOneDriveNewFileTriggerConfigName` to its name. The trigger config input accepts
  `%OnOneDriveNewFileTriggerConfigName%` or a literal name.
  Configure `ConnectorNamespace__pollingEndpoint`
  with the service-derived base endpoint and grant the host identity access to
  receive and acknowledge events.
- For local polling tests, stop the deployed app or use separate configs so the
  hosts do not compete for events.

## More connectors and events

Browse the [connector reference](https://aka.ms/connectors) and
[full Python samples](https://github.com/Azure-Samples/functions-connectors-python).
See the [OneDrive polling sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/onedriveApp/polling)
for other events and the [OneDrive webhook sample](https://github.com/Azure-Samples/functions-connectors-python/tree/main/onedriveApp/webhook)
for the Functions webhook delivery model.

> [!CAUTION]
> The handler logs file names, paths, and metadata. Use a test folder and restrict
> log access. Never commit tokens or private local settings.

## Programming Model V2

The Python V2 model defines triggers and bindings with decorators and supports
multiple functions in one file. You can also organize functions in blueprints
and register them with `app.register_functions()`.

To learn more, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide)
and [Python blueprints](https://aka.ms/azure-functions-python-blueprints).