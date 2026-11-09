using Azure.Connectors.Sdk.SharePointOnline.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class SharePointGetOnNewFileItems
{
    private readonly ILogger<SharePointGetOnNewFileItems> _logger;

    public SharePointGetOnNewFileItems(ILogger<SharePointGetOnNewFileItems> logger)
    {
        _logger = logger;
    }

    // Visit https://aka.ms/connectortrigger to learn how to use this trigger binding
    [Function(nameof(SharePointGetOnNewFileItems))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "%SharePointGetOnNewFileItemsTriggerConfigName%")]
        SharePointOnlineOnNewFileItemsTriggerPayload payload)
    {
        _logger.LogInformation("Received GetOnNewFileItems trigger ({Count} items)", payload.Body?.Value?.Count ?? 0);
    }

    // For other operations, use the matching trigger payload type in Azure.Connectors.Sdk.SharePointOnline.Models.
    //
    // GetOnUpdatedFileItems - SharePointOnlineOnUpdatedFileItemsTriggerPayload
    // GetOnNewItems - SharePointOnlineOnNewItemsTriggerPayload
    // GetOnUpdatedItems - SharePointOnlineOnUpdatedItemsTriggerPayload
    // GetOnChangedItems - SharePointOnlineOnChangedItemsTriggerPayload
    // GetOnDeletedItems - SharePointOnlineOnDeletedItemsTriggerPayload
    // GetOnDeletedFileItems - SharePointOnlineOnDeletedFileItemsTriggerPayload
    // GetOnNewItemsFromForm - SharePointOnlineOnNewItemsFromFormTriggerPayload
    // GetOnUpdatedFileClassifiedTimes - SharePointOnlineOnUpdatedFileClassifiedTimesTriggerPayload
    // GetOnRecurrenceDigest - SharePointOnlineOnRecurrenceDigestTriggerPayload
}
