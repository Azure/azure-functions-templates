const { connectors } = require('@azure/functions-extensions-connectors');

connectors.onedriveforbusiness.onNewFiles('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnOneDriveNewFileTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.files.length} OneDrive file(s).`);
    }
});
