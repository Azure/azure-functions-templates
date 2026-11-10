const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

connectors.teams.onNewChannelMessage('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewChannelMessageTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.messages.length} Teams channel message(s).`);
    }
});
