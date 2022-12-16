const { app } = require('@azure/functions');

app.serviceBusTopic('%functionName%', {
    connection: '%connection%',
    topicName: '%topicName%',
    subscriptionName: '%subscriptionName%',
    handler: (message, context) => {
        context.log('Service bus topic function processed message:', message);
    }
});
