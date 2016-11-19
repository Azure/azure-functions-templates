using System;

public static string Run(string input, string name, TraceWriter log)
{
    log.Info($"C# External trigger function processed file: " + name);
    return input;
}