using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Kusto;
using Microsoft.Extensions.Logging;

namespace Company.Function;

// Visit https://github.com/Azure/Webjobs.Extensions.Kusto/tree/main/samples/samples-outofproc/InputBindingSamples
// KustoInputBinding sample 
// Execute queries against the ADX cluster.
// Add `KustoConnectionString` to the local.settings.json
public class KustoInputBindingIsolated
{
    private readonly ILogger _logger;

    public KustoInputBindingIsolated(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<KustoInputBindingIsolated>();
    }

    [Function("KustoInputBindingIsolated")]
    public IEnumerable<Object> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        [KustoInput(Database: "DB",
            KqlCommand = "Table", // KQL to execute : declare query_parameters (records:int);Table | take records
            KqlParameters = "", // Parameters to bind : @records={records}
            Connection = "KustoConnectionString")] IEnumerable<Object> result)
    {
        _logger.LogInformation("C# HTTP trigger with Kusto Input Binding function processed a request.");
        return result;
    }
}