using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class DataverseSubscribeOnNewItems
{
    private readonly ILogger<DataverseSubscribeOnNewItems> _logger;

    public DataverseSubscribeOnNewItems(ILogger<DataverseSubscribeOnNewItems> logger)
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
    // Visit https://aka.ms/connectortrigger to learn how to use this trigger binding.
    [Function(nameof(DataverseSubscribeOnNewItems))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "TriggerConfigNameValue")]
        JsonElement payload)
    {
        _logger.LogInformation("Received SubscribeOnNewItems trigger");
    }

    // JsonElement preserves dynamic Dataverse fields that vary by table and custom columns.
    // For other Dataverse operations, keep JsonElement and point TriggerConfigName at a config
    // provisioned for that operation.
    //
    // SubscribeOnUpdatedItems - JsonElement
    // SubscribeOnDeletedItems - JsonElement
    // SubscribeWebhookTrigger - JsonElement
    // BusinessEventsTrigger - JsonElement
}
