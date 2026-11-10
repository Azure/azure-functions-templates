const { DeliveryMode } = require('@azure/functions');
const { connectorTrigger } = require('@azure/functions-extensions-connectors');

// Matches the legacy GetOnNewItems_V2 operation: new rows only, not all changes.
connectorTrigger('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnDataverseRowChangedTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.items.length} Dataverse new row(s).`);
    }
});
