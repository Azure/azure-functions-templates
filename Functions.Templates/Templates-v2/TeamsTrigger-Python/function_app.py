import logging

import azure.functions as func
# This example uses SDK types to directly access the underlying Connector object provided by the Connector trigger.
# To use, add azurefunctions-extensions-connectors to your requirements.txt file
import azurefunctions.extensions.connectors.teams as teams

# For more supported operations and SDK types, see:
# https://github.com/Azure/azure-functions-python-extensions/tree/dev/azurefunctions-extensions-connectors#key-concepts
# OnNewChannelMessage - list[teams.ChatMessage]
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

app = func.FunctionApp()


@app.function_name(name="$(FUNCTION_NAME_INPUT)")
# Visit aka.ms/connectortrigger to local testing and deployment instructions.
@app.connector_trigger(
    arg_name="messages",
    delivery_mode=func.DeliveryMode.POLL,
    connection="$(CONNECTION_INPUT)",
    trigger_config_name="$(ON_NEW_CHANNEL_MESSAGE_TRIGGER_CONFIG_INPUT)",
)
def $(FUNCTION_NAME_INPUT)(messages: list[teams.ChatMessage]) -> None:
    """Receive new channel messages."""
    logging.info("Received OnNewChannelMessage trigger (%s items)", len(messages))