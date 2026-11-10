const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

// Connector operations and normalized payload types (SharepointonlineExtensions):
// GetOnNewFileItems - Item[]
// GetOnUpdatedFileItems - Item[]
// GetOnNewItems - Item[]
// GetOnUpdatedItems - Item[]
// GetOnChangedItems - Item[]
// GetOnNewItemsFromForm - Item[]
// GetOnRecurrenceDigest - Item[]
// GetOnUpdatedFileClassifiedTimes - Item[]
// GetOnDeletedItems - DeletedItem[]
// GetOnDeletedFileItems - DeletedItem[]
// Use the matching typed helper or connectorTrigger<TItem> for another operation.
// Configure a Poll trigger config with that operation's required parameters.
connectors.sharepoint.onNewFileItems('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnSharepointNewFileTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.records.length} SharePoint file item(s).`);
    }
});
