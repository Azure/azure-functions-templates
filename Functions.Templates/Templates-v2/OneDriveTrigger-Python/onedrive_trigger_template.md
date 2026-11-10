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

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts

app = func.FunctionApp()

# Visit aka.ms/connectortrigger to local testing and deployment instructions.
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
	The function name input sets the complete handler name; its default is
	`onedrive_trigger_on_new_file_poll`. Choose a name that is unique in this app
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