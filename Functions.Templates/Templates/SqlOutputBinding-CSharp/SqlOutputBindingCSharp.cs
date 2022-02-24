using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SqlOutputBindingCSharp
    {
        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [FunctionName("SqlOutputBindingCSharp")]
         public static CreatedResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "addtodoitem")] HttpRequest req,
            [Sql("table", ConnectionStringSetting = "SqlConnectionString")] out ToDoItem output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            output = new ToDoItem
            {
                Id = "1",
                Priority = 1,
                Description = "Hello World"
            };

            return new CreatedResult($"/api/addtodoitem", output);
        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
