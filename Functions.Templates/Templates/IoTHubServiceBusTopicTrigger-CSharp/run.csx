using System;
using System.Threading.Tasks;

public static void Run(string mySbMsg, TraceWriter log)
{
    log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
}