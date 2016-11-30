# Azure Functions Templates
This repository contains the templates used by the [Azure Functions Portal](https://functions.azure.com/signin). Templates are pre-defined functions that demonstrate a working scenario and could be used as a starting point for more complex ones.

## Template Format
A template requires specific metadata files and folder structure so that [Azure Functions Portal](https://functions.azure.com/signin) can understand and graphically present it. Please find more information on individual files and their contents below.

- **Binding.json:** This is a metadata file for all the bindings and their possible configuration settings. It is common across all templates and is located [here](Bindings/bindings.json). It also contains metadata for binding related UI Elements and the corresponding text.

- **Function.json:** This file contains binding data specific to each template. It provides valid values for the possible settings on a binding.

- **Code file:** Code file holds the actual code executed by the template. The name of the file depends on the Target language used by the template. For all the languages, the file name is `run`, followed by the file extension specific to the language. Additionally Javascript also supports `index.js` as the name of the code file.

- **Metadata.json:** UI related metadata specific to each template is present here. For e.g. Template Name, category.

- **Sample.dat:** Sample.dat contains sample input data for each template. The Run text box in the portal will be populated by the contents of the sample.dat file.

## Testing templates

### Testing via Azure Functions Portal
1. Download and extract contents of [Template tools](https://github.com/Azure/azure-webjobs-sdk-templates/releases/download/0.9/Tools.zip) zip file.
2. Execute the following command from the root of the folder containing the extracted files.    
`CreateTemplateConfig.exe <TemplateRepositoryRootPath>`
3. The tool will generate the following files.
    - templates.json
    - bindings.json
4. Open [https://functions.azure.com](https://functions.azure.com) in chrome and login with your credentials.
5. Open developer Tools (F12) -> Application -> Local Storage
6. Expand `Local Storage` and select [https://functions.azure.com](https://functions.azure.com)
7. Create a new key `dev-bindings` and copy contents of `bindings.json` in the value column.
8. Create a new key `dev-templates` and copy contents of `templates.json` in the value column.
9. Refresh the portal page to reflect your template/binding changes.

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
