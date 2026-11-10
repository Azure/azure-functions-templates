const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

// Connector operations and normalized metadata payloads (OnedriveforbusinessExtensions):
// OnNewFilesV2 - BlobMetadata[]
// OnUpdatedFilesV2 - BlobMetadata[]
// OnNewFileV2 / OnUpdatedFileV2 return file content, not BlobMetadata.
// Use a raw app.connectorTrigger for file content instead of this metadata helper.
// Configure a Poll trigger config with the selected operation's required parameters.
connectors.onedriveforbusiness.onNewFiles('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnOneDriveNewFileTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.files.length} OneDrive file(s).`);
    }
});
