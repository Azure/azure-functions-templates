import type { InvocationContext } from '@azure/functions';
import { connectors, type SharepointItemTriggerContext } from '@azure/functions-extensions-connectors';

connectors.sharepoint.onNewFileItems('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnSharepointNewFileTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger: SharepointItemTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.records.length} SharePoint file item(s).`);
    }
});
