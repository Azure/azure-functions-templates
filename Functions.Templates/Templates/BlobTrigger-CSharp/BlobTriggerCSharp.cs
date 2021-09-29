using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerCSharp
    {
        private readonly ILogger<BlobTriggerCSharp> _logger;

        public BlobTriggerCSharp(ILogger<BlobTriggerCSharp> log)
        {
            _logger = log;
        }

        [FunctionName("BlobTriggerCSharp")]
        public void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")]Stream myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
