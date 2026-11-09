const { connectorTrigger } = require('@azure/functions-extensions-connectors');

// Matches the legacy GetOnNewItems_V2 operation: new rows only, not all changes.
connectorTrigger('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnDataverseRowChangedTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.items.length} Dataverse new row(s).`);
    }
});
