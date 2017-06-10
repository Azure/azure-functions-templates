# Azure Functions Templates
This repository contains the templates used by the [Azure Functions Portal](https://functions.azure.com/signin) and Visual Studio 2017 tooling. Templates are pre-defined functions that demonstrate a working scenario and could be used as a starting point for more complex ones.

## Template Format
A template requires specific metadata files and folder structure so that [Azure Functions Portal](https://functions.azure.com/signin) and Visual Studio 2017 tooling can understand and graphically present it. Certain C# templates cater only to portal whereas others are applicable to both Portal and Visual Studio 2017 tooling. The templates that are consumed by both hosts (Portal and Visual Studio) are structured differently.  Please find more information on template format, individual files and their contents below.

### Files used by portal
- **Binding.json:** This is a metadata file for all the bindings and their possible configuration settings. It is common across all templates and is located [here](/Functions.Templates/Bindings/bindings.json). It also contains metadata for binding related UI Elements and the corresponding text.
- **Function.json:** This file contains binding data specific to each template. It provides valid values for the possible settings on a binding.
- **Code file:** Code file holds the actual code executed by the template. The name of the file depends on the Target language used by the template. For all the languages, the file name is `run`, followed by the file extension specific to the language. Additionally, JavaScript also supports `index.js` as the name of the code file.
- **Metadata.json:** UI related metadata specific to each template is present here. For e.g. Template Name, category.
- **Sample.dat:** Sample.dat contains sample input data for each template. The Run text box in the portal will be populated by the contents of the sample.dat file.

### Files used by Visual Studio (Only applicable to C# templates)

- **run.cs:** The file holds the actual code for the templates along with the trigger and binding attributes required by Visual Studio tooling. Please note this a class file as opposed the C# script file (.csx) consumed by the portal.
- **.build.config/template.json:** This is consumed by the dotnet templating engine implemented by Visual Studio 2017.
- **.build.config/vs-2017.3.host.json** This is host config file for Visual Studio 2017 as required by the dotnet templating engine.

You can find more information on the templatting engine at the [wiki page](https://github.com/dotnet/templating/wiki) of the dotnet templating repository.
 
`Please note that as part of the packaging process the folder build.config is renamed to .template.config as required by the dotnet templating engine.`

## Build

### Generate templates for portal
1. Execute the [getTools](getTools.ps1) script from the root of the repository
2. Build the Functions.Templates/Functions.Templates.csproj via Visual studio or Execute `msbuild Functions.Templates.csproj` from Functions.Templates folder
3. The generated templates should be present in the `Functions.Templates\bin\Portal\Release\Azure.Functions.Templates.Portal` folder

### Generate templates for Visual Studio
1. Execute the [getTools](getTools.ps1) script from the root of the repository
2. Execute `msbuild Functions.Templates.csproj /target:VisualStudioTemplates` from Functions.Templates folder
3. The generated templates should be present in the `Functions.Templates\bin\VS\Release\` folder, Azure.Functions.Templates nuget pacakges

## Adding New Templates

### Adding new templates for portal only
1. Open Functions.Templates.sln
2. Portal at the minimum requires at least the following files for a template to be complete
    - function.json
    - metadata.json
    - run.csx
3. Add the respective files in the solution
4. Add the following entries to this [nuspec file](Functions.Templates/PortalTemplates.nuspec)
```XML
<file src="Templates/<TemplateFolderName>/<codeFile>" target="Templates/<TemplateFolderName>/<codeFile>" />
<file src="Templates/<TemplateFolderName>/function.json" target="Templates/<TemplateFolderName>/function.json" />
<file src="Templates/<TemplateFolderName>/metadata.json" target="Templates/<TemplateFolderName>/metadata.json" />
<file src="Templates/<TemplateFolderName>/project.json" target="Templates/<TemplateFolderName>/project.json" />
```
5. Make sure the strings present in metadata.json are added to the [Resource file](Functions.Templates/Resources/Resources.resx). Strings are reference by adding '$' before the string name. For example `$TimerTriggerCSharp_description` present in the [metadata.json](Functions.Templates/Templates/TimerTrigger-CSharp/metadata.json)
6. Build the solution, verify your template is present in the build output

### Adding new templates for Visual Studio only (C# Only)
1. Open Functions.Templates.sln
2. Visual Studio at the minimum requires at least the following files for a template to be complete    
    - run.cs
    - .build.config/template.json
    - .build.config/vs-2017.3.host.json
    - .build.config/vs-2017.3/function_f.png: Can be copied from the any C# template.
3. Add the respective files in the solution
4. Add the following entries to this [nuspec file](Functions.Templates/ItemTemplates.nuspec)
```XML
<file src="Templates/<TemplateFolderName>/run.cs" target="content/<TemplateFolderName>/run.cs" />
<file src="Templates/<TemplateFolderName>/build.config/template.json" target="content/<TemplateFolderName>/.template.config/template.json" />
<file src="Templates/<TemplateFolderName>/build.config/vs-2017.3.host.json" target="content/<TemplateFolderName>/.template.config/vs-2017.3.host.json" />
<file src="Templates/<TemplateFolderName>/build.config/vs-2017.3/function_f.png" target="content/<TemplateFolderName>/.template.config/vs-2017.3/function_f.png" />
```
5. Build the solution, verify your template is present in the build output

### Adding new templates for portal and Visual Studio (C# only)
1. Follow the steps to create template for portal and for Visual Studio
2. Merge the contents of the code file for portal and Visual Studio as done in the sample file below
```CSHARP
#if (portalTemplates) // Code applicable to portal only
using System;
public static void Run(TimerInfo myTimer, TraceWriter log)
#endif
#if (vsTemplates) // Code applicable to Visual Studio
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class TimerTriggerCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static void Run([TimerTrigger("ScheduleValue")]TimerInfo myTimer, TraceWriter log)
#endif
// Code common to portal and Visual Studio
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
#if (vsTemplates)
    }
}
#endif
```

## Testing 

### Testing portal templates
1. Execute the [getTools](getTools.ps1) script from the root of the repository
2. Build the Functions.Templates/Functions.Templates.csproj via Visual studio
3. The build solution will generate the following files in the Functions.Templates\bin\Portal\Test.
    - templates.json
    - bindings.json
4. Open [https://functions.azure.com](https://functions.azure.com) in chrome and login with your credentials.
5. Open developer Tools (F12) -> Application -> Local Storage
6. Expand `Local Storage` and select [https://functions.azure.com](https://functions.azure.com)
7. Create a new key `dev-bindings` and copy contents of `bindings.json` in the value column.
8. Create a new key `dev-templates` and copy contents of `templates.json` in the value column.
9. Refresh the portal page to reflect your template/binding changes.

`Note: Newly added string will not appear in the portal when testing with this method`

## License

This project is under the benevolent umbrella of the [.NET Foundation](http://www.dotnetfoundation.org/) and is licensed under [the MIT License](LICENSE.txt)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Related Github Repositories
- [Azure Functions Portal](https://github.com/projectkudu/AzureFunctionsPortal)
- [WebJobs Script SDK](https://github.com/Azure/azure-webjobs-sdk-script/)
- [WebJobs SDK Extensions](https://github.com/Azure/azure-webjobs-sdk-extensions)

## Contribute Code or Provide Feedback
If you would like to become an active contributor to this project please follow the instructions provided in [Microsoft Azure Projects Contribution Guidelines](http://azure.github.com/guidelines.html).
If you encounter any bugs with the templates please file an issue in the [Issues](https://github.com/Azure/azure-webjobs-sdk-templates/issues) section of the project.