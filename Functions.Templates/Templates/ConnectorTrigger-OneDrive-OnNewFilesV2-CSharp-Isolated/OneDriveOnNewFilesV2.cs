using Azure.Connectors.Sdk.OneDriveForBusiness.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class OneDriveOnNewFilesV2
{
    private readonly ILogger<OneDriveOnNewFilesV2> _logger;

    public OneDriveOnNewFilesV2(ILogger<OneDriveOnNewFilesV2> logger)
    {
        _logger = logger;
    }

    // Visit https://aka.ms/connectortrigger to learn how to use this trigger binding
    [Function(nameof(OneDriveOnNewFilesV2))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "%OneDriveOnNewFilesV2TriggerConfigName%")]
        OneDriveForBusinessOnNewFilesTriggerPayload payload)
    {
        _logger.LogInformation("Received OnNewFilesV2 trigger ({Count} items)", payload?.Body?.Value?.Count ?? 0);
    }

    // For other operations, use the matching trigger payload type in Azure.Connectors.Sdk.OneDriveForBusiness.Models.
    //
    // OnNewFileV2 - string
    // OnUpdatedFileV2 - string
    // OnUpdatedFilesV2 - OneDriveForBusinessOnUpdatedFilesTriggerPayload
}
