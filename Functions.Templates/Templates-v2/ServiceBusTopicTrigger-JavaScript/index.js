const { app } = require('@azure/functions');

app.serviceBusTopic('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    topicName: '$(TOPIC_NAME_INPUT)',
    subscriptionName: '$(SUBSCRIPTION_NAME_INPUT)',
    handler: (message, context) => {
        context.log('Service bus topic function processed message:', message);
    }
});
