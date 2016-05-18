using System;

public static void Run(string input, out string output, TraceWriter log)
{
    log.Info($"C# SaaS trigger function processed a file!");
    
    output = input;
}