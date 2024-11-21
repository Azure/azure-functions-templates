using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class SqlInputBindingHttpTriggerCSharp
{
    private readonly ILogger _logger;

    public SqlInputBindingHttpTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SqlInputBindingHttpTriggerCSharp>();
    }

    [Function("SqlInputBindingHttpTriggerCSharp")]
    public IEnumerable<Object> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        [SqlInput("SELECT * FROM object",
        "SqlConnectionString")] IEnumerable<Object> result)
    {
        _logger.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

        return result;
    }
}