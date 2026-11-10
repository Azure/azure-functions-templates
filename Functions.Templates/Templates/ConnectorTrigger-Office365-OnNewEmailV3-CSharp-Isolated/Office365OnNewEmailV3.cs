using Azure.Connectors.Sdk.Office365.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class Office365OnNewEmailV3
{
    private readonly ILogger<Office365OnNewEmailV3> _logger;

    public Office365OnNewEmailV3(ILogger<Office365OnNewEmailV3> logger)
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
    [Function(nameof(Office365OnNewEmailV3))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "TriggerConfigNameValue")]
        Office365OnNewEmailTriggerPayload payload)
    {
        _logger.LogInformation("Received OnNewEmailV3 trigger ({Count} items)", payload?.Body?.Value?.Count ?? 0);
    }

    // For other operations, use the matching trigger payload type in Azure.Connectors.Sdk.Office365.Models.
    //
    // OnFlaggedEmailV4 - Office365OnFlaggedEmailTriggerPayload
    // OnNewMentionMeEmailV3 - Office365OnNewEmailMentioningMeTriggerPayload
    // CalendarGetOnNewItemsV3 - Office365OnCalendarNewItemsTriggerPayload
    // CalendarGetOnUpdatedItemsV3 - Office365OnCalendarUpdatedItemsTriggerPayload
    // OnUpcomingEventsV3 - Office365OnUpcomingEventsTriggerPayload
    // SharedMailboxOnNewEmailV2 - Office365OnSharedMailboxNewEmailTriggerPayload
}
