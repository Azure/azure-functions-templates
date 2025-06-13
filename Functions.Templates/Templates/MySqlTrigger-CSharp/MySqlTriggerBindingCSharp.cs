using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class MySqlTriggerBinding
    {
        [FunctionName("MySqlTriggerBindingCSharp")]
        public static void Run(
                [MySqlTrigger("table1", "MySqlConnectionString")] IReadOnlyList<MySqlChange<ToDoItem>> changes,
                ILogger log)
        {
            log.LogInformation("MySql Changes: ");
            // The output is used to inspect the trigger binding parameter in test methods.
            foreach (MySqlChange<ToDoItem> change in changes)
            {
                ToDoItem toDoItem = change.Item;
                log.LogInformation($"Change operation: {change.Operation}");
                log.LogInformation($"Id: {toDoItem.Id}, Priority: {toDoItem.Priority}, Description: {toDoItem.Description}");
            }
        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
