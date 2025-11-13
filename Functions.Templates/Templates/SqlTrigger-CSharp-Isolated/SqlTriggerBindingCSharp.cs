using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function;

public class SqlTriggerBindingCSharp(ILogger<SqlTriggerBindingCSharp> logger)
{
    // Visit https://aka.ms/sqltrigger to learn how to use this trigger binding
    [Function("SqlTriggerBindingCSharp")]
    public void Run(
        [SqlTrigger("table", "SqlConnectionString")] IReadOnlyList<SqlChange<ToDoItem>> changes,
            FunctionContext context)
    {
        logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

    }
}

public class ToDoItem
{
    public string Id { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
}