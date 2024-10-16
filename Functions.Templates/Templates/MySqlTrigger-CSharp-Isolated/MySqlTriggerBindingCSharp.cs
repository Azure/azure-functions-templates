using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.MySql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class MySqlTriggerBindingCSharp
    {
        private readonly ILogger _logger;

        public MySqlTriggerBindingCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MySqlTriggerBindingCSharp>();
        }

        [Function("MySqlTriggerBindingCSharp")]
        public void Run(
            [MySqlTrigger("table", "MySqlConnectionString")] IReadOnlyList<MySqlChange<ToDoItem>> changes,
                FunctionContext context)
        {
            _logger.LogInformation("MySql Changes: " + JsonConvert.SerializeObject(changes));

        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
