# ReplaceTokensInText

## Definition

Replaces placeholders in a body of text with their associated values. Non-recursive.

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal | :white_check_mark: |
| Core Tools   | :white_check_mark: |
| VS Code      | :white_check_mark: |

## Parameters

**`type`**: `ReplaceTokensInText`

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
The template placeholder to assign the transformed text to. Format: `$(PLACEHOLDER_ID)`

**`source`** String  
The template placeholder whose associated text should have its placeholder tokens replaced.

## Summary

All placeholders in the text of `source` defined at the time this action executes will be replaced with their associated text.  
Placeholders that have not been defined at the time this action executes will _not_ be replaced.

## Exceptions

## Examples

The following example template defines a job which performs the following actions:

1. Read the text of `function_body.py` into `$(TIMER_FUNCTION_BODY)`
2. Replace placeholder tokens `$(FUNCTION_NAME_INPUT)` and `$(SCHEDULE_INPUT)` in the text of `$(TIMER_FUNCTION_BODY)` with their associated values and assign the transformed text back to `$(TIMER_FUNCTION_BODY)`.
3. Read the text of `function_app.py` into `$(FUNCTION_APP)`
4. Write the text of `$(FUNCTION_APP)` into a file, replacing the `$(TIMER_FUNCTION_BODY)` placeholder token with the transformed text in `$(TIMER_FUNCTION_BODY)` in the process.

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
            "name": "Create New Project with Timer Function",
            "type": "CreateNewApp",
            "inputs": [
                {
                    "assignTo": "$(APP_FILENAME)",
                    "paramId": "app-FileName"
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
                "replaceText_FunctionBody",
                "readFileContent_FunctionApp",
                "writeFile_FunctionApp"
            ]
        },
    ],
    "actions": [
        {
            "name": "readFileContent_FunctionApp",
            "type": "GetTemplateFileContent",
            "assignTo": "$(FUNCTION_APP)",
            "filePath": "function_app.py"
        },
        {
            "name": "readFileContent_FunctionBody",
            "type": "GetTemplateFileContent",
            "assignTo": "$(TIMER_FUNCTION_BODY)",
            "filePath": "function_body.py"
        },
        {
            "name": "replaceText_FunctionBody",
            "type": "ReplaceTokensInText",
            "assignTo": "$(TIMER_FUNCTION_BODY)",
            "source": "$(TIMER_FUNCTION_BODY)"
        },
        {
            "name": "writeFile_FunctionApp",
            "type": "WriteToFile",
            "filePath": "$(APP_FILENAME)",
            "source": "$(FUNCTION_APP)",
            "replaceTokens": true,
            "FileExtension": ".py"
        }
    ]    
}
```

<details>

<summary>Template Code Files</summary>

### `function_app.py`

```python
import datetime

import logging

import azure.functions as func

app = func.FunctionApp()

$(TIMER_FUNCTION_BODY)
```

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

## Remarks

Although most template scenarios can be accomplished by setting `"replaceTokens": true` on the [`AppendToFile`](AppendToFile.md) and [`WriteToFile`](WriteToFile.md) actions, more complex sequences of actions may require an 'in-memory' token replacement.

Like the `replaceTokens` property of the aforementioned actions, `ReplaceTokensInText` does not result in a recursive operation.
