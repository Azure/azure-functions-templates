using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class BlobTriggerCSharp
    {
        [Function("BlobTriggerCSharp")]
        public static void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")] string myBlob, string name,
            FunctionContext context)
        {
            var logger = context.GetLogger("BlobTriggerCSharp");
            logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
    }
}
