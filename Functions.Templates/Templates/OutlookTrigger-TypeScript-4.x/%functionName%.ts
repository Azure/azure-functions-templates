import { DeliveryMode, type InvocationContext } from '@azure/functions';
import { connectors, type EmailTriggerContext } from '@azure/functions-extensions-connectors';

connectors.office365.onNewEmail('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewEmailTriggerConfigName%',
    handler: async (trigger: EmailTriggerContext, context: InvocationContext): Promise<void> => {
        context.log(`Received ${trigger.emails.length} Outlook email(s).`);
    }
});
