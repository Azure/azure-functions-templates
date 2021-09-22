using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerCSharp
    {
        private readonly ILogger<BlobTriggerCSharp> log;

        public BlobTriggerCSharp(FunctionContext context)
        {
            this.log = context.GetLogger("BlobTriggerCSharp");
        }

        [Function("BlobTriggerCSharp")]
        public void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")] string myBlob, string name)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
    }
}
