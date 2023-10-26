using System;
using System.Collections.Generic;

public static void Run(IReadOnlyList<ToDoItem> input, ILogger log)
{
    if (input != null && input.Count > 0)
    {
        log.LogInformation("Documents modified " + input.Count);
        log.LogInformation("First document Id " + input[0].id);
    }
}

// Customize the model with your own desired properties
public class ToDoItem
{
    public string id { get; set; }
    public string Description { get; set; }
}
