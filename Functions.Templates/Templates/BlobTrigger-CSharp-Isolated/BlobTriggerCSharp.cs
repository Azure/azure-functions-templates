using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerCSharp
    {
        private readonly ILogger _logger;

        public BlobTriggerCSharp(ILogger<BlobTriggerCSharp> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobStringFunction))]
        public void BlobStringFunction(
            [BlobTrigger(PathValue), Connection = "ConnectionValue"] string data)
        {
            _logger.LogInformation("Blob content: {content}", data);
        }
    }
}
