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

        [Function(nameof(BlobStringFunction))]
        public void BlobStringFunction(
            [BlobTrigger("string-trigger")] string data)
        {
            _logger.LogInformation("Blob content: {content}", data);
        }
    }
}
