# Overview
This repository is home to a collection of templates used development tools to provide a quick start experience for Azure Functions. A template in this context is a sample function that demonstrates use of one or more bindings supported by Azure Functions. Here are the development tools that use templates from this repository:

- [Azure Function Core Tools](https://github.com/Azure/azure-functions-core-tools)
- [Azure Portal](https://portal.azure.com)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Visual Studio](https://visualstudio.microsoft.com/vs/)

## Build Status
|Branch|Status|
|---|---|
|dev|[![Build Status](https://azfunc.visualstudio.com/Azure%20Functions/_apis/build/status/Azure.azure-functions-templates?branchName=dev)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=43&branchName=dev)
|master|[![Build Status](https://azfunc.visualstudio.com/Azure%20Functions/_apis/build/status/Azure.azure-functions-templates?branchName=master)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=43&branchName=master)
|v3.x|[![Build Status](https://azfunc.visualstudio.com/Azure%20Functions/_apis/build/status/Azure.azure-functions-templates?branchName=master)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=43&branchName=v3.x)
|v1.x|[![Build status](https://ci.appveyor.com/api/projects/status/9u3okk4mruuwt4dl/branch/v1.x?svg=true)](https://ci.appveyor.com/project/appsvc/azure-webjobs-sdk-templates/branch/v1.x)|

# Getting Started 
This section is focused on building templates for Azure Functions. If you are not familiar with Azure Functions, please refer to the following documentation to learn about technical concepts and components surrounding Azure Functions.
 -  [Azure Functions overview](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview) 
 -  [Azure Functions developers guide](https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference)

## Repository Structure
```
root
|
└─── Functions.Templates
|    |
|    └─── Templates
|    |
|    └─── ProjectTemplate
|    |
|    └─── Resources
|    |
|    └─── Bindings
└─── Tools
|    |
|    └─── CheckResourceStrings
|  
└───Build
```


## Requirements
- [Node (10.x)](https://nodejs.org/dist/latest-v10.x/)
- [Gulp](https://gulpjs.com/docs/en/getting-started/quick-start)

## Build Steps
```
cd Build
npm install
gulp build-all
```

> These build steps only work on Windows

# Template files
The templates includes metadata files in addition to the files required to execute a function. The metadata files help drive the user interface and development experience for creating a function using a template. You can find information on the metadata files in the section below:

- **Resources.resx:** This file contains all the localized resource strings referenced in the metadata files. The strings are used for description, help, error text and other display text for the UI elements of the development tools. Strings in resources.resx file are reference by adding `$` before the corresponding string name. For example `TimerTriggerCSharp_description` is present in resources.resx file and is referenced in metadata.json file as `$TimerTriggerCSharp_description`

- **Sample.dat:** Sample.dat contains sample input data for each template.

- **Metadata.json:** This file includes basic information that explains the purpose of the template. It also includes configuration properties that help drive the UI required to create a function using a template. Individual properties are explain inline.

  ```Javascript
  {
      "defaultFunctionName": "TimerTrigger",      // Default name to be used for a function if the user does not provide one during deployment.
      "description": "$TimerTrigger_description", // Short description explaining the functionality of the generated function.
      "name": "Timer trigger",                    // The template name shown in UI.
      "language": "C#",
      "category": [                               // Category under which this template should be presented.
          "$temp_category_core", 
          "$temp_category_dataProcessing"
      ],
      "categoryStyle": "timer",                  // Category style used to pick the correct icon for the template.
      "enabledInTryMode": true,                  // Should this template be available in try mode: https://tryfunctions.com/ng-min/try?trial=true
      "userPrompt": [                            // The development tools will prompt to configure this setting during template deployment
          "schedule"
      ]
  }
  ```

- **Bindings.json:** This file contains metadata for all the configuration options available for all the bindings. This allows the development tools to provide the users with an option to configure additional settings for the bindings used by the template. It also drives to UI used to add / modify bindings of an existing functions. Here is a sample entry for timerTrigger binding.

  ```Javascript
  {
    "type": "timerTrigger",                                     // The binding type property matching the "type" property in function.json
    "displayName": "$timerTrigger_displayName",                 // This is the text used by the UI element to display binding name.
    "direction": "trigger", 
    "enabledInTryMode": true,                                   // Should this binding be available in try mode https://tryfunctions.com/ng-min/try?trial=true
    "documentation": "$content=Documentation\\timerTrigger.md", // Location of the documentation related to this binding in the templates repository
    "settings": [                                               
        {
          "name": "schedule",
          "value": "string",
          "defaultValue": "0 * * * * *",
          "required": true,
          "label": "$timerTrigger_schedule_label",              // display text for the config option
          "help": "$timerTrigger_schedule_help",                // help text explaining what the config option is
          "validators": [
            {
              "expression": "",                                 // regex that can be used to validate the configuration value
              "errorText": "$timerTrigger_schedule_errorText"   // help text in case the regex validation fails
            }
          ]
        }
      ]
  }
  ```

# Adding a new template

1. At the minimum, following three files are required for it to be a valid template.
    - function.json
    - metadata.json
    - run
2. Add the following entries to this [nuspec file](Build/PackageFiles/Templates.nuspec)

```XML
<file src="Templates/<TemplateFolderName>/<CodeFileName>" target="Templates/<TemplateFolderName>/run.<ext>" />
<file src="Templates/<TemplateFolderName>/function.json" target="Templates/<TemplateFolderName>/function.json" />
<file src="Templates/<TemplateFolderName>/metadata.json" target="Templates/<TemplateFolderName>/metadata.json" />
<file src="Templates/<TemplateFolderName>/sample.dat" target="Templates/<TemplateFolderName>/sample.dat" />
```
3. Make sure the strings present in metadata.json are added to the [Resource file](Functions.Templates/Resources/Resources.resx). Strings are reference by adding '$' before the string name. For example `$TimerTriggerCSharp_description` present in the [metadata.json](Functions.Templates/Templates/TimerTrigger-CSharp/metadata.json)


# Adding dotnet templates
Dotnet used in Visual studio are driven by dotnet templating engine. Follow this [documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates) to learn how to create dotnet templates. You can find more information on the templating engine at the [wiki page](https://github.com/dotnet/templating/wiki) of the dotnet templating repository.
- After you have created a dotnet template, Add the file entries to the ItemTemplates [nuspec file](Build/PackageFiles/ItemTemplates.nuspec).

# Adding templates to extension bundle
1. Follow the steps mentioned in the [adding a new template](#adding-a-new-template) section.
2. Add the file entries to the [ExtensionBundleTemplates-1.x nuspec file](Build/PackageFiles/ExtensionBundleTemplates-1.x.nuspec) or [ExtensionBundleTemplates-2.x nuspec file](Build/PackageFiles/ExtensionBundleTemplates-2.x.nuspec) depending on your requirement.

# Testing templates

## Azure Function Core Tools
1. Once the template files have been added / updated, build the templates using the [Build Steps](#build-steps)
2. Locate the built templates at `..\bin\Templates`
3. Locate the install location of core tools by executing `where func` from command line.
4. Locate the templates directory typically present here `nodejs\node_modules\azure-functions-core-tools\bin\templates`
5. Replace the contents of the templates directory with the one found in Step 4.
6. Create a new function app using `func init . --no-bundle`
7. Select non-dotnet runtime

## Visual Studio
1. Once the template files have been added / updated, build the templates using the [Build Steps](#build-steps)
2. Make sure all instances of Visual Studio are closed
2. Open the `LastKnownGood` found at `%userprofile%\AppData\Local\AzureFunctionsTools\Tags\v2`
3. Note the release version present in the file
4. Open the templates folder `%userprofile%\AppData\Local\AzureFunctionsTools\Releases\<releaseVersion>\templates` matching the release version found in step 2. (Note: if adding the files to the release version folder found in `LastKnownGood` doesn't work, then try other release version folders.)
5. Replace the contents of the folder with the one found in `..\bin\VS`
6. Rename `Microsoft.Azure.WebJobs.ItemTemplates.X.0.0.nupkg` to `ItemTemplates.nupkg`
7. Delete the `%userprofile%\.templateengine` directory
8. Select `Azure Functions v2 (.NET Core)` when creating a new function app via Visual Studio 

## Extension bundle template via Azure Function Core Tools
1. Once the template files have been added / updated, build the templates using the [Build Steps](#build-steps)
2. Locate the built templates at `..\bin\ExtensionBundle.Templates-v2`
3. Replace the contents of the `StaticContent\v1` directory for the bundle you want to test
    - Sample Location: `%userprofile%\AppData\Local\Temp\Functions\ExtensionBundles\Microsoft.Azure.Functions.ExtensionBundle\2.0.1\StaticContent\v1`
4. Create a new function app using `func init .` (If you are testing .csx files then run `func init . --csx`)
5. Create a new function using  `func new` (If you are testing .csx files then run `func new --csx`)
6. Start a function app by running `func host start` or `func start`

# License

This project is under the benevolent umbrella of the [.NET Foundation](http://www.dotnetfoundation.org/) and is licensed under [the MIT License](LICENSE.txt)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Related Github Repositories
- [Azure Function Core Tools](https://github.com/Azure/azure-functions-core-tools)
- [Azure Portal](https://github.com/Azure/azure-functions-ux)
- [Visual Studio Code](https://github.com/microsoft/vscode-azurefunctions)
- [Azure Function Host](https://github.com/Azure/azure-functions-host)
- [WebJobs SDK Extensions](https://github.com/Azure/azure-webjobs-sdk-extensions)

## Contribute Code or Provide Feedback
If you would like to become an active contributor to this project please follow the instructions provided in [Microsoft Azure Projects Contribution Guidelines](http://azure.github.com/guidelines.html).
If you encounter any bugs with the templates please file an issue in the [Issues](https://github.com/Azure/azure-webjobs-sdk-templates/issues) section of the project.
