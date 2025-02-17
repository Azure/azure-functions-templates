using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Kusto;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class KustoOutputBindingIsolated
{
    private readonly ILogger _logger;

    public KustoOutputBindingIsolated(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<KustoOutputBindingIsolated>();
    }

    // Visit https://github.com/Azure/Webjobs.Extensions.Kusto/tree/main/samples/samples-outofproc/OutputBindingSamples 
    // KustoOutputBinding sample 
    // Execute queries against the ADX cluster.
    // Add `KustoConnectionString` to the local.settings.json
    [Function("KustoOutputBindingIsolated")]
    [KustoOutput(Database: "DB", // The database to ingest the data into , e.g. functionsdb
        TableName = "TargetTable", // Table to ingest data into, e.g. Storms
        Connection = "KustoConnectionString")]
    public async Task<ToDoItem> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger with Kusto Output Binding function processed a request.");
        ToDoItem todoitem = await req.ReadFromJsonAsync<ToDoItem>() ?? new ToDoItem
            {
                Id = "1",
                Priority = 1,
                Description = "Hello World"
            };
        return todoitem;
    }
}

public class ToDoItem
{
    public string Id { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
}