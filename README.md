# Overview
This repository is home to a collection of templates used by development tools to provide a quick start experience for Azure Functions. A template in this context is a sample  that demonstrates use of one or more bindings supported by Azure Functions. Following are the development tools that use templates from this repository:

- [Azure Function Core Tools](https://github.com/Azure/azure-functions-core-tools)
- [Azure Portal](https://portal.azure.com)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Visual Studio](https://visualstudio.microsoft.com/vs/)

Dotnet templates are consumed by Visual Studio and Visual Studio code via tooling feed. Non-dotnet and C# Script templates are consumed via extension bundles.

## Build Status
|Branch|Status|Description|
|---|---|---|
|dev|[![Build Status](https://azfunc.visualstudio.com/Azure%20Functions/_apis/build/status/Azure.azure-functions-templates?branchName=dev)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=43&branchName=dev)| This is the primary development branch all pull request go against this branch. |
|master|[![Build Status](https://azfunc.visualstudio.com/Azure%20Functions/_apis/build/status/Azure.azure-functions-templates?branchName=master)](https://azfunc.visualstudio.com/Azure%20Functions/_build/latest?definitionId=43&branchName=master)| This is the deployment branch all releases are performed from this branch. |

## Build Requirements
- [Node (10.x)](https://nodejs.org/dist/latest-v10.x/)
- [Gulp](https://gulpjs.com/docs/en/getting-started/quick-start)

## Build Steps
```
cd Build
npm install
gulp build-all
```
> These build steps only work on Windows

## Dotnet templates

There are two kind of dotnet templates contained within this repository, script type (.csx and .fsx) templates that do not require compilation and non-script type (.cs and .fs) templates that require compilation. 
  
## Creating a dotnet templates (.cs and .fs)
Template for dotnet precompiled functions apps adheres to the specification provided by the dotnet templating engine. The dotnet templating engine or an implementation of one is present within each of the dotnet client and is responsible for consuming dotnet templates. This format is not specific to Azure Functions but is a standard used for all dotnet templates by VS, VS Code and dotnet cli. This section covers some basic information needed to add a pre-compiled template. 

There are 2 kinds of dotnet templates.
1. Project templates: Project templates are responsible for creating initial set of files needed to build and run the project. For azure functions this would include, csproj file, host.json, local.settings.json file and so on.
2. Item templates: Item templates are templates include files that you would want to add to an existing project. For azure functions this would mean class files, new functions.

### Template files:
At the minimum you need the following files for a valid dotnet template. Please refer to the this link for [detailed documentation](https://github.com/dotnet/templating/tree/main/docs) on each of the files and properties contained within the file.

1.  **.template.config/template.json** : Presence of this file within the folder structure indicates to the dotnet templating engine that this is a template. This file contains symbols and post action action configuration that is used to generate a function from the template. The key difference between project and item template file is that `tags -> type` property would say project vs item for corresponding template types. Below is sample file with comments on individual fields.
2.  **.template.config/vs-2017.3.host** : This file contains information required to generate UI elements in Visual studio. For example, label and help text for UI elements.
3. **.template.config/vs-2017.3/*.png**: Icon files for menu items in Visual studio.
4. **Class file or Project file** : Class file is required if you are creating an item template. This could be either a `.cs` file or a `.fs` file. Project file is require if you are creating a project template. This could be either a `.csproj` file or an `.fsproj` file.

Here is a sample PR adding a dotnet item template. https://github.com/Azure/azure-functions-templates/pull/1162


### Adding a dotnet template for release to Visual Studio / Visual Studio code
This section covers information you need to add your template to the list of templates that show up within Visual studio and Visual studio code. VS and VS code only support templates for a single major version of a particular extension. That means there currently is no way to simultaneously include templates that target different major versions of the same extension within the same template list. The build system in this repository uses `.nuspec files` to manage different release trains. Add your template to the nuspec file corresponding to the target runtime release based on the table below.

|Nuspec File |Description|
|---|---|
|[ProjectTemplates_v3.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ProjectTemplates_v3.x.nuspec) | Project templates for dotnet in-proc function app targeting runtime v3 |
|[ItemTemplates_v3.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ItemTemplates_v3.x.nuspec) | Item templates for dotnet in-proc function app targeting runtime v3 |
|[ProjectTemplates-Isolated_v3.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ProjectTemplates-Isolated_v3.x.nuspec) | Project templates for dotnet isolated (out of proc) function app targeting runtime v3 |
|[ItemTemplates-Isolated_v3.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ItemTemplates-Isolated_v3.x.nuspec) | Item templates for dotnet isolated (out of proc) function app targeting runtime v3 |
|[ProjectTemplates_v4.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ProjectTemplates_v4.x.nuspec) | Project templates for dotnet in-proc function app targeting runtime v4 |
|[ItemTemplates_v4.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ItemTemplates_v4.x.nuspec) | Item templates for dotnet in-proc function app targeting runtime v4 |
|[ProjectTemplates-Isolated_v4.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ProjectTemplates-Isolated_v4.x.nuspec) | Project templates for dotnet isolated (out of proc)  function app targeting runtime v4 |
|[ItemTemplates-Isolated_v4.x.nuspec](Build/PackageFiles/Dotnet_precompiled/ItemTemplates-Isolated_v4.x.nuspec) | Item templates for dotnet isolated (out of proc)  function app targeting runtime v4 |

### Testing dotnet templates
Dotnet pre-compiled templates are currently hosted by the following clients. Please follow the instructions in this section to test the corresponding clients.

#### Visual Studio (VS 2019 and VS 2022):

1. Once the template files have been added / updated, build the templates using the [Build Steps](#build-steps)
2. Make sure all instances of Visual Studio are closed;
3. Open the directory `%userprofile%\AppData\Local\AzureFunctionsTools\Tags`
4. Each of the directory within the tags directory represent a runtime, for testing runtime `v4` templates open directory `v4`
5. Open the `LastKnownGood` file in the directory for the runtime version you want to test and note the release version present in the file
6. Open the templates output directory, `..\bin\VS`
    > To test dotnet-inproc, copy `Microsoft.Azure.WebJobs.ItemTemplates.X.X.X` and `Microsoft.Azure.WebJobs.ProjectTemplates.X.X.X`.
  
    > To test dotnet-isolated, copy `Microsoft.Azure.Functions.Worker.ItemTemplates.X.0.0` and `Microsoft.Azure.Functions.Worker.ProjectTemplates.X.0.0`.
7. Open the templates cache directory for release version matching the one found in step 5: `%userprofile%\AppData\Local\AzureFunctionsTools\Releases\<releaseVersion>` 
8. Open the `templates` folder for the framework you want to test:
    1. For in-proc, use the `templates` folder found at the root of the templates cache directory
    2. For net7-isolated, use the `net7-isolated/templates` folder (for isolated, you should see a folder for netfx, net6, net5 etc.)
9.  Replace the contents of the folder with the copied packages found in `..\bin\VS`
10. Delete the `%userprofile%\.templateengine` directory
11. Select corresponding function runtime when creating a new function app via Visual Studio 
12. Run through the test scenarios

#### Core tools

1. Clone the azure-functions-core-tools repo
    ```
    git clone https://github.com/Azure/azure-functions-core-tools.git
    ```
2. Build the azure-functions-core-tools and publish it with below [commands](https://github.com/Azure/azure-functions-core-tools/blob/v4.x/build.sh).
    ```
    cd azure-functions-core-tools
    dotnet build Azure.Functions.Cli.sln
    ```
    For windows
    ```
    dotnet publish src/Azure.Functions.Cli/Azure.Functions.Cli.csproj --runtime win-x64 --output /tmp/cli
    ```
    For Linux
    ```
    dotnet publish src/Azure.Functions.Cli/Azure.Functions.Cli.csproj --runtime linux-x64 --output /tmp/cli
    ```

    This step will create a core-tool cli in `C:\tmp\cli`, create `templates` folder inside cli folder.

3. Add/update your templates, build the templates using the [Build Steps](#build-steps)
4. For in-proc templates
    1. Open output directory `..\bin\VS` after build and copy `Microsoft.Azure.WebJobs.ItemTemplates.X.X.X.nupkg`, `Microsoft.Azure.WebJobs.ProjectTemplates.X.0.0.nupkg` 
    
    2. Paste above copied nuget packages to `C:\tmp\cli\templates`
5. For testing net-isolated templates, copy `Microsoft.Azure.Functions.Worker.ItemTemplates.X.X.X`, `Microsoft.Azure.Functions.Worker.ProjectTemplates.X.X.X.nupkg` to `C:\tmp\cli\templates\net-isolated`. Please create `net-isolated` folder inside templates folder.
6. Delete the `%userprofile%\.templateengine` directory

7. Now to create Azure function with your template run the following and select your template.
    ```
    c:\tmp\cli\func new
    ```
8. Visual Studio Code: We currently do not have a way to test templates in VS code without going to through extensive set up. Will update this section with instructions once we have the right set of hooks enabled.

## Creating script type templates
Script type templates are templates for functions that do not require a compilation step. The templates includes metadata files in addition to the files required to execute a function. The metadata files help drive the user interface and development experience for creating a function using a template. In addition to the metadata file you would also need to add a code file for the corresponding language in the template. You can find information on the metadata files in the section below:

### Template files:
1. **Code file**: This is the file that contains the function execution code. This could be Python, JavaScript (Node JS), PowerShell, CSharp Script, FSharp Script. The only time this file is not needed is when you are creating a template for custom handlers.

2. **Metadata.json:** This file includes basic information that explains the purpose of the template. It also includes configuration properties that help drive the UI required to create a function using a template. Individual properties are explain inline.

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

3. **Resources.resx:** This file contains all the localized resource strings referenced in the metadata files. The strings are used for description, help, error text and other display text for the UI elements of the development tools. Strings in resources.resx file are reference by adding `$` before the corresponding string name. For example `TimerTriggerCSharp_description` is present in resources.resx file and is referenced in metadata.json file as `$TimerTriggerCSharp_description`

4. **Bindings.json:** This file contains metadata for all the configuration options available for all the bindings. This allows the development tools to provide the users with an option to configure additional settings for the bindings used by the template. It also drives to UI used to add / modify bindings of an existing functions. Here is a sample entry for timerTrigger binding. You only need to add a template for binding that does not exist in binding.json.

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
5. **Sample.dat:** Sample.dat contains sample input data for each template.

### Adding a template to Extension bundle
Pretty much all non-dotnet templates do not require compilation. The only exception to this is java templates which are not part of this repository as of now. Non-dotnet templates, CSharp and FSharp script templates are deployed via Extension bundles. This means that a new version of these templates would be deployed when a new version of extension bundle is released. Similar to dotnet templates we use .nuspec files to control which templates are included in which package (in this case extension bundle). Following tables list all the .nuspec files and their corresponding bundles. 

|Nuspec File |Description|
|---|---|
|[ExtensionBundleTemplates-1.x.nuspec](Build/PackageFiles/ExtensionBundle/ExtensionBundleTemplates-1.x.nuspec) | Templates for Extension bundle v1 |
|[ExtensionBundleTemplates-2.x.nuspec](Build/PackageFiles/ExtensionBundle/ExtensionBundleTemplates-2.x.nuspec) | Templates for Extension bundle v2 |
|[ExtensionBundleTemplates-3.x.nuspec](Build/PackageFiles/ExtensionBundle/ExtensionBundleTemplates-3.x.nuspec) | Templates for Extension bundle v3 |
|[ExtensionBundlePreviewTemplates-3.x.nuspec](Build/PackageFiles/ExtensionBundle/ExtensionBundlePreviewTemplates-3.x.nuspec) | Templates for preview Extension bundle v3 |
|[ExtensionBundlePreviewTemplates-4.x.nuspec](Build/PackageFiles/ExtensionBundle/ExtensionBundlePreviewTemplates-4.x.nuspec) | Templates for preview Extension bundle v4 |

### Testing script type template via Core tools
1. Once the template files have been added / updated, build the templates using the [Build Steps](#build-steps)
2. Locate the zip file for built template in the bin directory `..\bin\`
3. Extract the zip file content you want to test. This be based on the nuspec file you updated. 
3. Create a function app via core tools, open host.json to verify that it has extension bundle configuration present.
    - Sample commands for node app: `func init . --worker-runtime node`
4. Execute the `func GetExtensionBundlePath` to find the path to the bundle being used.
    - Sample response: `%userprofile%\.azure-functions-core-tools\Functions\ExtensionBundles\Microsoft.Azure.Functions.ExtensionBundle\2.8.4`
5. Replace the contents of the `StaticContent\v1` directory (path from step 5) with the files extracted from the zip file in step 3.
6. Execute `func new` at the root of the sample app to see the new / updated templates.

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
