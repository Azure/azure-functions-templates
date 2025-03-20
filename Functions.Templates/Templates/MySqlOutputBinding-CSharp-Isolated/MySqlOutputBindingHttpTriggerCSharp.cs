using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class MySqlOutputBindingHttpTriggerCSharp
{
    private readonly ILogger _logger;

    public MySqlOutputBindingHttpTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MySqlOutputBindingHttpTriggerCSharp>();
    }

    [Function("MySqlOutputBindingHttpTriggerCSharp")]
    [MySqlOutput("table", "MySqlConnectionString")]
    public async Task<ToDoItem> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger with MySql Output Binding function processed a request.");

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