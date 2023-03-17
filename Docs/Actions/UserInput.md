# UserInput

## Definition

Prompts the user for input and assigns it to a placeholder as a `string`.

### Tooling Support

| Environment  | Support |
| ------------ | ------- |
| Azure Portal | :white_check_mark: |
| Core Tools   | :white_check_mark: |
| VS Code      | :white_check_mark: |

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

**`assignTo`** String  
The placeholder to assign the user-provided input to. Format: `$(PLACEHOLDER_ID)`

**`paramId`** String
Lookup key for additional metadata about the property whose value the user is prompted for, ex. binding metadata, function name validation, etc. Should match the `name` of an entry in [userPrompts.json](../../Functions.Templates/Bindings/userPrompts.json).

## Effects

The value provided by the user via tooling will be assigned to the placeholder specified by `assignTo`.

## Exceptions

## Examples

## Remarks

Input that can be acquired prior to iterating through a job's actions list should be defined in a `template.json` file's `input` section. However, if an input prompt depends on the result of an action in a job's action list, the prompt would need to be defined as a subsequent `UserInput` action.