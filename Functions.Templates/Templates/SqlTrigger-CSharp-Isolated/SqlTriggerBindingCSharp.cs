using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SqlTriggerBindingCSharp
    {
        private readonly ILogger _logger;

        public SqlTriggerBindingCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SqlTriggerBindingCSharp>();
        }

        [Function("SqlTriggerBindingCSharp")]
        public static void Run(
            [SqlTrigger("table", "SqlConnectionString")] IReadOnlyList<SqlChange<ToDoItem>> changes,
                FunctionContext context)
        {
            _logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
