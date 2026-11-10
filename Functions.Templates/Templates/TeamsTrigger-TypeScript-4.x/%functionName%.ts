import { DeliveryMode, type InvocationContext } from '@azure/functions';
import { connectors, type ChannelMessageTriggerContext } from '@azure/functions-extensions-connectors';

// Connector operations and normalized payload types (TeamsExtensions):
// OnNewChannelMessage - ChatMessage[]
// OnNewChannelMessageMentioningMe - ChatMessage[]
// OnGroupMembershipAdd - OnGroupMemberChangeResponse
// OnGroupMembershipRemoval - OnGroupMemberChangeResponse
// WebhookChatMessageTrigger - ChatMessageWebhookResponseSchema[]
// WebhookNewMessageTrigger - DynamicNewMessageWebhookTriggerResponseSchema[]
// WebhookAtMentionTrigger - DynamicAtMentionWebhookTriggerResponseSchema[]
// WebhookKeywordTrigger - DynamicKeywordWebhookTriggerResponseSchema[]
// WebhookMessageReactionTrigger - MessageReactionWebhookResponseSchema[]
// RecordingTrigger - RecordingWebhookResponseSchema[]
// TranscriptTrigger - TranscriptWebhookResponseSchema[]
// OnGroupMemberChangeResponse is already an array of records.
// Use the matching typed helper or connectorTrigger<TItem> for another operation.
// An operation name containing Webhook does not select Functions delivery mode.
// Configure a Poll trigger config with that operation's required parameters.
connectors.teams.onNewChannelMessage('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewChannelMessageTriggerConfigName%',
    handler: async (trigger: ChannelMessageTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.messages.length} Teams channel message(s).`);
    }
});
