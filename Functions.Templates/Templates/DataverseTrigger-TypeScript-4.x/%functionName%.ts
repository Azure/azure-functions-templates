import { DeliveryMode, type InvocationContext } from '@azure/functions';
import type { Item } from '@azure/connectors/generated/CommondataserviceExtensions';
import { connectorTrigger, type ConnectorTriggerContext } from '@azure/functions-extensions-connectors';

// Connector operations and normalized payload types (CommondataserviceExtensions):
// GetOnNewItems_V2 - Item[]
// GetOnUpdatedItems_V2 - Item[]
// The selected legacy Admin Only operation is deprecated and receives new rows only.
// GetOnDeletedItems_V2, SubscribeWebhookTrigger and BusinessEventsTrigger are not
// exported by @azure/connectors@0.4.0-preview; do not assume Python types exist here.
// Configure a Poll trigger config with the selected operation's required parameters.
connectorTrigger<Item>('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnDataverseRowChangedTriggerConfigName%',
    handler: async (trigger: ConnectorTriggerContext<Item>, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.items.length} Dataverse new row(s).`);
    }
});
