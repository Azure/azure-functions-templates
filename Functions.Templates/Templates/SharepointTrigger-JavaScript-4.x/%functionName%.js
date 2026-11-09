const { connectors } = require('@azure/functions-extensions-connectors');

connectors.sharepoint.onNewFileItems('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnSharepointNewFileTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.records.length} SharePoint file item(s).`);
    }
});
