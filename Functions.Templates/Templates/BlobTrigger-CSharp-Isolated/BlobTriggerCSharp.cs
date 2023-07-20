using System;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerCSharp
    {
        private readonly ILogger<BlobTriggerCSharp> _logger;

        public BlobTriggerCSharp(ILogger<BlobTriggerCSharp> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobStreamFunction))]
        public async Task Run(
            [BlobTrigger(PathValue, Connection = "ConnectionValue")] Stream myBlob)
        {
            using var blobStreamReader = new StreamReader(stream);
            _logger.LogInformation("Blob content: {content}", await blobStreamReader.ReadToEndAsync());
        }
    }
}
