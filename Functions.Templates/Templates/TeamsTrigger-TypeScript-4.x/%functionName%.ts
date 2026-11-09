import type { InvocationContext } from '@azure/functions';
import { connectors, type ChannelMessageTriggerContext } from '@azure/functions-extensions-connectors';

connectors.teams.onNewChannelMessage('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewChannelMessageTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger: ChannelMessageTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.messages.length} Teams channel message(s).`);
    }
});
