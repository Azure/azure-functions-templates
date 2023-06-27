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

        [Function(nameof(BlobClientFunction))]
        public async Task Run(
            [BlobTrigger(PathValue), Connection = "ConnectionValue"] BlobClient client)
        {
            var downloadResult = await client.DownloadContentAsync();
            var blobContent = downloadResult.Value.Content.ToString();
            _logger.LogInformation("Blob content: {content}", blobContent);
        }
    }
}
