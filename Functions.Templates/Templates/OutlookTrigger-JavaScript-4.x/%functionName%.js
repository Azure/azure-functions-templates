const { connectors } = require('@azure/functions-extensions-connectors');

connectors.office365.onNewEmail('%functionName%', {
    deliveryMode: 'Poll',
    connection: 'ConnectorNamespace',
    triggerConfigName: '%OnNewEmailTriggerConfigName%',
    cardinality: 'one',
    handler: async (trigger, context) => {
        context.log(`Received ${trigger.emails.length} Outlook email(s).`);
    }
});
