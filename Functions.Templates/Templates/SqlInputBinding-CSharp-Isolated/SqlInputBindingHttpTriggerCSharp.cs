using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SqlInputBindingHttpTriggerCSharp
    {
        private readonly ILogger _logger;

        public SqlInputBindingHttpTriggerCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SqlInputBindingHttpTriggerCSharp>();
        }

        [Function("SqlInputBindingCSharp")]
        public static IEnumerable<Object> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [SqlInput("SELECT * FROM object",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result)
        {
            _logger.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            return result;
        }
    }
}
