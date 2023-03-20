# GetTemplateFileContent

## Definition

Opens a file, reads all the text in the file, and then assigns the text to a template's placeholder.

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal | :white_check_mark: |
| Core Tools   | :white_check_mark: |
| VS Code      | :white_check_mark: |

## Parameters

**`type`**  
**`GetTemplateFileContent`**

<!-- vvv Common Parameters vvv -->
**`name`** String  
The arbitrary identifier for an action, used to reference it from a job's list of actions.

**`conditions`** array _(optional)_  
Collection of conditionals that must evaluate to `true` for this action to be executed. See [Conditionals](../conditionals.md) for more information.

**`continueOnError`** boolean _(optional)_  
Whether to continue the parent job's execution if this action errors. Default: `false`

**`errorText`** String _(optional)_  
The text to display if this action errors (ex. manual instructions).
<!-- ^^^ Common Parameters ^^^ -->

**`assignTo`** String  
The placeholder to assign the text from the file at `filePath` to. Format: `$(PLACEHOLDER_ID)`

**`filePath`** String  
The file path to read text from, relative to the root directory of the template that defines this action. Does not support subdirectories.

## Effects

The text from the file at `filePath` will be assigned to the placeholder defined by `assignTo`.

## Exceptions

## Examples

The following example template defines a job which gets the text in `function_app.py`.

When the `getTemplateFileContent_FunctionApp` action executes, tooling with read the contents of `function_app.py` and assigns it to the placeholder `$(FUNCTION_APP_CONTENT)`. The `writeFile_FunctionApp` action then writes the text associated with `$(FUNCTION_APP_CONTENT)` to a file.

### `template.json`

```json
{
    "author": "Microsoft",
    "name": "HTTP Trigger",
    "description": "$HttpTrigger_description",
    "programmingModel": "v2",
    "language": "python",
    "jobs": [
        {
            "name": "Create New Project with httpTrigger Function",
            "type": "CreateNewApp",
            "inputs": [
                {
                    "assignTo": "$(APP_FILENAME)",
                    "paramId": "app-fileName",
                    "required": true
                },
                {
                    "assignTo": "$(FUNCTION_NAME_INPUT)",
                    "paramId": "trigger-functionName",
                    "required": true,
                    "defaultValue": "HttpTrigger"
                },
                {
                    "assignTo": "$(AUTHLEVEL_INPUT)",
                    "paramId": "httpTrigger-authLevel",
                    "required": true,
                    "defaultValue": "function"
                }
            ],
            "actions": [
                "getTemplateFileContent_FunctionApp",
                "writeFile_FunctionApp"
            ]
        }
    ],
    "actions" : [
        {
            "name": "getTemplateFileContent_FunctionApp",
            "type": "GetTemplateFileContent",
            "assignTo": "$(FUNCTION_APP_CONTENT)",
            "filePath": "function_app.py"
        },
        {
            "name": "writeFile_FunctionApp",
            "type": "WriteToFile",
            "source": "$(FUNCTION_APP_CONTENT)",
            "filePath": "$(APP_FILENAME)",
            "continueOnError": false,
            "errorText": "Unable to create the function app",
            "replaceTokens": true
        }
    ]
}
```

### `function_app.py`

```python
import azure.functions as func
import logging

app = func.FunctionApp(auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))

@app.function_name(name="$(FUNCTION_NAME_INPUT)")
@app.route(route="$(FUNCTION_NAME_INPUT)")
def $(FUNCTION_NAME_INPUT)(req: func.HttpRequest) -> func.HttpResponse:
     logging.info('Python HTTP trigger function processed a request.')

     name = req.params.get('name')
     if not name:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('name')

     if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
     else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
```

## Remarks
