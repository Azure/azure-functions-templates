using Azure.Connectors.Sdk.Commondataservice.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class DataverseGetOnNewItemsV2
{
    private readonly ILogger<DataverseGetOnNewItemsV2> _logger;

    public DataverseGetOnNewItemsV2(ILogger<DataverseGetOnNewItemsV2> logger)
    {
        _logger = logger;
    }

    // For local development, add these under "Values" in local.settings.json; in Azure, add them as app settings:
#if (TriggerConfigIsSetting)
    // "ConnectionValue__pollingEndpoint": "https://<namespace-host>",
    // "TriggerConfigSettingNameValue": "<trigger-config-name>"
#else
    // "ConnectionValue__pollingEndpoint": "https://<namespace-host>"
#endif
    // Visit https://aka.ms/connectortrigger to learn how to use this trigger binding
    [Function(nameof(DataverseGetOnNewItemsV2))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "TriggerConfigNameValue")]
        CommondataserviceOnNewItemsTriggerPayload payload)
    {
        _logger.LogInformation("Received GetOnNewItems_V2 trigger");
    }

    // For other operations, use the matching trigger payload type in Azure.Connectors.Sdk.Commondataservice.Models.
    //
    // GetOnUpdatedItems_V2 - CommondataserviceOnUpdatedItemsTriggerPayload
    // GetOnDeletedItems_V2 - CommondataserviceOnDeletedItemsTriggerPayload
    // SubscribeWebhookTrigger - JsonElement
    // BusinessEventsTrigger - JsonElement
}
