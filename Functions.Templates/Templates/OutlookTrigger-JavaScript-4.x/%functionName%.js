const { DeliveryMode } = require('@azure/functions');
const { connectors } = require('@azure/functions-extensions-connectors');

connectors.office365.onNewEmail('%functionName%', {
    deliveryMode: DeliveryMode.Poll,
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewEmailTriggerConfigName%',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.emails.length} Outlook email(s).`);
    }
});
