const { app } = require('@azure/functions');

app.serviceBusTopic('%functionName%', {
    connection: '',
    topicName: 'mytopic',
    subscriptionName: 'mysubscription',
    handler: (message, context) => {
        context.log('Service bus topic function processed message:', message);
    }
});
