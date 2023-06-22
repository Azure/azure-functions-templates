# Kusto Output Binding - C<span>#</span>

The `Kusto Output Binding` makes it easy to ingest data into Kusto.

## How it works

For a `Kusto Output Binding` to work, the Table to ingest along with the Database where this table is defined need to be provided as input. For additional details please refer the [README](https://github.com/Azure/Webjobs.Extensions.Kusto/blob/main/README.md) in the repository.


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