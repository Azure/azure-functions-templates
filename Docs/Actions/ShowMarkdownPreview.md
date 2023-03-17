# ShowMarkdownPreview

## Definition

Displays a rendered Markdown file to the user.

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal | :x: |
| Core Tools   | :x: |
| VS Code      | :white_check_mark: |

## Parameters

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

**`filePath`** String
The file path of the Markdown file, relative to the root directory of the template that defines this action. Does not support subdirectories.

## Effects

Tooling displays the rendered Markdown to the user.

## Exceptions

## Examples

The following example template defines a job which appends the text of a file to the file selected by a user and displays the rendered `timer_trigger_template.md` Markdown file to the user.

### `template.json`

```json
{
    "author": "Microsoft",

    "name": "Timer Trigger",
    "description": "$TimerTrigger_description",
    "programmingModel": "v2",
    "language": "python",
    "jobs": [
        {
            "name": "Append Timer Function to File",
            "type": "AppendToFile",
            "input": [
                {
                    "assignTo": "$(SELECTED_FILEPATH)",
                    "paramId": "app-selectedFileName"
                },
                {
                    "assignTo": "$(FUNCTION_NAME_INPUT)",
                    "paramId": "trigger-functionName",
                    "defaultValue": "TimerTrigger"
                },                    
                {
                    "assignTo": "$(SCHEDULE_INPUT)",
                    "paramId": "timerTrigger-schedule",
                    "defaultValue": "0 */5 * * * *"
                }
            ],
            "actions": [
                "readFileContent_FunctionBody",
                "appendFunctionBody_SelectedFile",
                "showMarkdownPreview"
            ]
        }
    ]
    "actions": [
        {
            "name": "readFileContent_FunctionBody",
            "type": "GetTemplateFileContent",
            "assignTo": "$(TIMER_FUNCTION_BODY)",
            "filePath": "function_body.py"
        },
        {
            "name": "appendFunctionBody_SelectedFile",
            "type": "AppendToFile",
            "createIfNotExists" : false,
            "source": "$(TIMER_FUNCTION_BODY)",
            "filePath": "$(SELECTED_FILEPATH)",
            "continueOnError" : false,
            "errorText": "Unable to add template",
            "replaceTokens": true
        },
        {
            "name": "showMarkdownPreview",
            "type": "ShowMarkdownPreview",
            "filePath": "timer_trigger_template.md"
        },
    ]
}
```

<details>

<summary>Template Code Files</summary>

### `function_body.py`

```python
@app.function_name(name="$(FUNCTION_NAME_INPUT)")
@app.schedule(schedule="$(SCHEDULE_INPUT)", arg_name="myTimer", run_on_startup=True,
              use_monitor=False) 
def $(FUNCTION_NAME_INPUT)(myTimer: func.TimerRequest) -> None:
    utc_timestamp = datetime.datetime.utcnow().replace(
        tzinfo=datetime.timezone.utc).isoformat()

    if myTimer.past_due:
        logging.info('The timer is past due!')

    logging.info('Python timer trigger function ran at %s', utc_timestamp)
```

</details>

<details>

<summary>Template Markdown</summary>

### `timer_trigger_template.md`

```md
# Azure Functions: Timer Trigger in Python

## Timer Trigger

A timer trigger lets you run a function on a schedule.

## Using the Template

Following is an example code snippet for Timer Trigger using the [Python programming model V2](https://aka.ms/pythonprogrammingmodel) (currently in Preview).

````
```python
import datetime

import logging

import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="mytimer")
@app.schedule(schedule="0 */5 * * * *", arg_name="mytimer", run_on_startup=True,
              use_monitor=False) 
def test_function(mytimer: func.TimerRequest) -> None:
    utc_timestamp = datetime.datetime.utcnow().replace(
        tzinfo=datetime.timezone.utc).isoformat()

    if mytimer.past_due:
        logging.info('The timer is past due!')

    logging.info('Python timer trigger function ran at %s', utc_timestamp)
```
````

</details>

To run the code snippet generated through the command palette, note the following:

- The function application is defined and named `app`.
- Confirm that the parameters within the trigger reflect values that correspond with your storage account.
- The name of the file must be `function_app.py`.

## Programming Model V2 (Preview)

The new programming model in Azure Functions Python delivers an experience that aligns with Python development principles, and subsequently with commonly used Python frameworks. 

The improved programming model requires fewer files than the default model, and specifically eliminates the need for a configuration file (`function.json`). Instead, triggers and bindings are represented in the `function_app.py` file as decorators. Moreover, functions can be logically organized with support for multiple functions to be stored in the same file. Functions within the same function application can also be stored in different files, and be referenced as blueprints.

To learn more about using the new Python programming model for Azure Functions, see the [Azure Functions Python developer guide](https://aka.ms/pythondeveloperguide). Note that in addition to the documentation, [hints](https://aka.ms/functions-python-hints) are available in code editors that support type checking with PYI files.

To learn more about the new programming model for Azure Functions in Python, see [Programming Models in Azure Functions](https://aka.ms/functions-programming-models).
```

## Remarks
