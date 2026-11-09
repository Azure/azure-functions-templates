import type { InvocationContext } from '@azure/functions';
import { connectors, type OneDriveFileTriggerContext } from '@azure/functions-extensions-connectors';

connectors.onedriveforbusiness.onNewFiles('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnOneDriveNewFileTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger: OneDriveFileTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.files.length} OneDrive file(s).`);
    }
});
