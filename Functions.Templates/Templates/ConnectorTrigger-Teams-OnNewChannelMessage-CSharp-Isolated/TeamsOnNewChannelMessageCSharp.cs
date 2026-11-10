using Azure.Connectors.Sdk.Teams.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Connector;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class TeamsOnNewChannelMessageCSharp
{
    private readonly ILogger<TeamsOnNewChannelMessageCSharp> _logger;

    public TeamsOnNewChannelMessageCSharp(ILogger<TeamsOnNewChannelMessageCSharp> logger)
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
    [Function(nameof(TeamsOnNewChannelMessageCSharp))]
    public void Run(
        [ConnectorTrigger(
            DeliveryMode = ConnectorTriggerDeliveryMode.Poll,
            Connection = "ConnectionValue",
            TriggerConfigName = "TriggerConfigNameValue")]
        TeamsOnNewChannelMessageTriggerPayload payload)
    {
        _logger.LogInformation("Received OnNewChannelMessage trigger ({Count} items)", payload.Body?.Value?.Count ?? 0);
    }

    // For other operations, use the matching trigger payload type in Azure.Connectors.Sdk.Teams.Models.
    //
    // OnNewChannelMessageMentioningMe - TeamsOnNewChannelMessageMentioningMeTriggerPayload
    // OnGroupMembershipAdd - TeamsOnTeamMemberAddedTriggerPayload
    // OnGroupMembershipRemoval - TeamsOnTeamMemberRemovedTriggerPayload
    // WebhookChatMessageTrigger - string
    // WebhookNewMessageTrigger - string
    // WebhookAtMentionTrigger - string
    // WebhookKeywordTrigger - string
    // WebhookMessageReactionTrigger - string
    // RecordingTrigger - string
    // TranscriptTrigger - string
}
