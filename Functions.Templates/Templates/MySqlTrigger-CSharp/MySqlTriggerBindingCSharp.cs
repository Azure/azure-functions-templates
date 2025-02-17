using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.MySql
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Company.Function
{
    public static class MySqlTriggerBinding
    {
        [FunctionName("MySqlTriggerBindingCSharp")]
        public static void Run(
                [MySqlTrigger("table", "MySqlConnectionString")] IReadOnlyList<MySqlChange<ToDoItem>> changes,
                ILogger log)
        {
            log.LogInformation("MySql Changes: " + JsonConvert.SerializeObject(changes));

        }
    }

    public class ToDoItem
    {
        public string Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}
