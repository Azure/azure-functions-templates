using System.IO;
using System.Threading.Tasks;
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

        [Function(nameof(BlobTriggerCSharp))]
        public async Task Run(
            [BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
