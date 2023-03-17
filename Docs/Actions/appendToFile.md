# AppendToFile

## Definition

Appends text to an existing file in the user's workspace, or to a new file if the specified file does not exist.

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal | :white_check_mark: |
| Core Tools   | :white_check_mark: |
| VS Code      | :white_check_mark: |

### Parameters

**`type`**  
**`AppendToFile`**

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
The absolute path to the file in the user workspace to append the text of `source` to.

**`source`** String  
The text to append to the file at `filePath`.

**`createIfNotExists`** boolean _(optional)_  
Whether to create the file at `filePath` if it doesn't exist. Default: `false`

**`replaceTokens`** boolean  
Whether to replace tokens in the file at `filePath` after appending text from `source`. This operation is not recursive.

### Effects

The text of `source` will be appended to the file at `filePath`.  
If `filePath` does not exist and `createIfNotExists` is `true`, `filePath` will be created.  
If `replaceTokens` is true, any placeholders in the text of `source` will be replaced with their associated values as part of the operation.

### Exceptions

### Examples

The following example template defines a job which appends the text of a file to a file selected by the user. When the `AppendToFile` action
`appendFunctionBody_SelectedFile` executes, the text of `function_body.py` will be appended to the file `$(SELECTED_FILEPATH)` in the user's workspace.

Because `appendFunctionBody_SelectedFile`'s `replaceTokens` property is `true`, the placeholders `$(FUNCTION_NAME_INPUT)` and `$(SCHEDULE_INPUT)` in
the file at `$(SELECTED_FILEPATH)` will be replaced with their associated values (provided by tooling) after the text of `function_body.py` is appended.

#### `template.json`

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
                "appendFunctionBody_SelectedFile"
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
        }
    ]
}
```

#### `function_body.py`

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

### Remarks
