# ACTION_NAME

## Definition

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal |         |
| Core Tools   |         |
| VS Code      |         |

<!-- 
Emoji Shortcode        Meaning
:white_check_mark:     supported
:x:                    not supported
-->

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

## Effects

## Exceptions

## Examples

## Remarks
