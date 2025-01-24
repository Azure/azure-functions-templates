using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class EventGridBlobTriggerCSharp
{
    private readonly ILogger<EventGridBlobTriggerCSharp> _logger;

    public EventGridBlobTriggerCSharp(ILogger<EventGridBlobTriggerCSharp> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EventGridBlobTriggerCSharp))]
    public async Task Run([BlobTrigger("PathValue/{name}", Source = BlobTriggerSource.EventGrid, Connection = "ConnectionValue")] Stream stream, string name)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        _logger.LogInformation("C# Blob Trigger (using Event Grid) processed blob\n Name: {name} \n Data: {content}", name, content);
    }
}
