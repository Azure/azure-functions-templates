const { connectors } = require('@azure/functions-extensions-connectors');

connectors.teams.onNewChannelMessage('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewChannelMessageTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.messages.length} Teams channel message(s).`);
    }
});
