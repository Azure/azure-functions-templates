const { app } = require('@azure/functions');

app.serviceBusTopic('%functionName%', {
    connection: '%connection%',
    topicName: '%topicName%',
    subscriptionName: '%subscriptionName%',
    handler: (context, message) => {
        context.log('Service bus topic function processed message:', message);
    }
});
