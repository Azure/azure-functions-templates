using System;

public static void Run(string myIoTHubMessage, ILogger log)
{
    log.LogInformation($"C# IoT Hub trigger function processed a message: {myIoTHubMessage}");
}