const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

connectors.sharepoint.onNewFileItems('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnSharepointNewFileTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.records.length} SharePoint file item(s).`);
    }
});
