using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.MySql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class MySqlOutputBindingCSharp
    {
        [FunctionName("MySqlOutputBindingCSharp")]
        public static async Task<CreatedResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [MySql("table", "MySqlConnectionString")] IAsyncCollector<ToDoItem> output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with MySql Output Binding function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToDoItem todoitem = JsonConvert.DeserializeObject<ToDoItem>(requestBody) ?? new ToDoItem
                {
                    Id = "1",
                    Priority = 1,
                    Description = "Hello World"
                };
            await output.AddAsync(todoitem);

            return new CreatedResult(req.Path, todoitem);
        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
