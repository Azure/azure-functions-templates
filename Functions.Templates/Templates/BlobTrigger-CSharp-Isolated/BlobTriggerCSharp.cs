using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerCSharp
    {
        private readonly ILogger _logger;

        public BlobTriggerCSharp(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BlobTriggerCSharp>();
        }

        [Function("BlobTriggerCSharp")]
        public void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        }
    }
}
