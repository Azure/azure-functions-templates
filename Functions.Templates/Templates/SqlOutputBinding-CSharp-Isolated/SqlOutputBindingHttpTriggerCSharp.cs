using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class SqlOutputBindingHttpTriggerCSharp
    {
        private readonly ILogger _logger;

        public SqlOutputBindingHttpTriggerCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SqlOutputBindingHttpTriggerCSharp>();
        }

        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [Function("SqlOutputBindingHttpTriggerCSharp")]
        [SqlOutput("table", "SqlConnectionString")]
        public async Task<ToDoItem> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

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
}
