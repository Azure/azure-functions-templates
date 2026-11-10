import { DeliveryMode, type InvocationContext } from '@azure/functions';
import type { Item } from '@azure/connectors/generated/CommondataserviceExtensions';
import { connectorTrigger, type ConnectorTriggerContext } from '@azure/functions-extensions-connectors';

// Matches the legacy GetOnNewItems_V2 operation: new rows only, not all changes.
connectorTrigger<Item>('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnDataverseRowChangedTriggerConfigName%',
    handler: async (trigger: ConnectorTriggerContext<Item>, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.items.length} Dataverse new row(s).`);
    }
});
