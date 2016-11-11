using System;

public static void Run(string input, string name, out string output, TraceWriter log)
{
    log.Info($"C# External trigger function processed file: " + name);
    
    output = input;
}