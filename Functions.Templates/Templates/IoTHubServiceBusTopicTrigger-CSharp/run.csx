using System;
using System.Threading.Tasks;

public static void Run(string mySbMsg, ILogger log)
{
    log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
}