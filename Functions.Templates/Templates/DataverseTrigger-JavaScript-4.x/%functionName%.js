const { DeliveryMode } = require('@azure/functions');
const { connectorTrigger } = require('@azure/functions-extensions-connectors');

// Connector operations and normalized payload types (CommondataserviceExtensions):
// GetOnNewItems_V2 - Item[]
// GetOnUpdatedItems_V2 - Item[]
// The selected legacy Admin Only operation is deprecated and receives new rows only.
// GetOnDeletedItems_V2, SubscribeWebhookTrigger and BusinessEventsTrigger are not
// exported by @azure/connectors@0.4.0-preview; do not assume Python types exist here.
// Configure a Poll trigger config with the selected operation's required parameters.
connectorTrigger('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnDataverseRowChangedTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.items.length} Dataverse new row(s).`);
    }
});
