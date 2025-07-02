using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class MySqlTriggerBindingCSharp
{
    private readonly ILogger _logger;

    public MySqlTriggerBindingCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MySqlTriggerBindingCSharp>();
    }

    [Function("MySqlTriggerBindingCSharp")]
    public void Run(
        [MySqlTrigger("table1", "MySqlConnectionString")] IReadOnlyList<MySqlChange<ToDoItem>> changes,
            FunctionContext context)
    {
        _logger.LogInformation("MySql Changes: ");
        // The output is used to inspect the trigger binding parameter in test methods.
        foreach (MySqlChange<ToDoItem> change in changes)
        {
            ToDoItem toDoItem = change.Item;
            _logger.LogInformation($"Change operation: {change.Operation}");
            _logger.LogInformation($"Id: {toDoItem.Id}, Priority: {toDoItem.Priority}, Description: {toDoItem.Description}");
        }
    }
}

public class ToDoItem
{
    public string Id { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
}