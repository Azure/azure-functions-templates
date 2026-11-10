const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

connectors.onedriveforbusiness.onNewFiles('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnOneDriveNewFileTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.files.length} OneDrive file(s).`);
    }
});
