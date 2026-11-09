import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.teams as teams

# For other operations, use the matching trigger payload type from
# azurefunctions.extensions.connectors.teams.
#
# OnNewChannelMessageMentioningMe - list[teams.ChatMessage]
# OnGroupMembershipAdd - list[teams.OnGroupMemberChangeResponse]
# OnGroupMembershipRemoval - list[teams.OnGroupMemberChangeResponse]
# WebhookChatMessageTrigger - list[teams.ChatMessageWebhookResponseSchema]
# WebhookNewMessageTrigger - list[teams.DynamicNewMessageWebhookTriggerResponseSchema]
# WebhookAtMentionTrigger - list[teams.DynamicAtMentionWebhookTriggerResponseSchema]
# WebhookKeywordTrigger - list[teams.DynamicKeywordWebhookTriggerResponseSchema]
# WebhookMessageReactionTrigger - list[teams.MessageReactionWebhookResponseSchema]
# RecordingTrigger - list[teams.RecordingWebhookResponseSchema]
# TranscriptTrigger - list[teams.TranscriptWebhookResponseSchema]
# An operation name containing Webhook does not select Functions delivery mode.
# Configure a trigger config for the selected operation and its required parameters.

app = func.FunctionApp()


@app.function_name(name="$(FUNCTION_NAME_INPUT)_on_new_channel_message_poll")
@app.connector_trigger(
    arg_name="messages",
    delivery_mode=func.DeliveryMode.POLL,
    connection="$(CONNECTION_INPUT)",
    trigger_config_name="$(ON_NEW_CHANNEL_MESSAGE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)_on_new_channel_message_poll(messages: list[teams.ChatMessage]) -> None:
    """Receive new channel messages."""
    logging.info("Received OnNewChannelMessage trigger (%s items)", len(messages))