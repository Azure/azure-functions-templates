using System;

public static string Run(string inputFile, string name, TraceWriter log)
{
    log.Info($"C# External trigger function processed file: " + name);
    return inputFile;
}