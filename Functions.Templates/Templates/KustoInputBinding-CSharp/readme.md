# Kusto Input Binding - C<span>#</span>

The `Kusto Input Binding` makes it easy to retrieve data from a database, returning the output of the KQL query or a KQL function.

## How it works

For a `Kusto Input Binding` to work, the KqlCommand has to be provided along with KqlParameter (optional) for running the command. For additional details please refer the [README](https://github.com/Azure/Webjobs.Extensions.Kusto/blob/main/README.md) in the repository.


Add `KustoConnectionString` to your `local.settings.json`

_local.settings.json_
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "KustoConnectionString": "Data Source=https://<cluster>.kusto.windows.net;Database=<database>;Fed=True;AppClientId=<app-id>;AppKey=<app-key>;Authority Id=<tenant-id>"
  }
}
```