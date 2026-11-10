import { DeliveryMode, type InvocationContext } from '@azure/functions';
import { connectors, type ChannelMessageTriggerContext } from '@azure/functions-extensions-connectors';

connectors.teams.onNewChannelMessage('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewChannelMessageTriggerConfigName%',
    handler: async (trigger: ChannelMessageTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.messages.length} Teams channel message(s).`);
    }
});
