const { app } = require('@azure/functions');

app.serviceBusQueue('%functionName%', {
    connection: '',
    queueName: 'myinputqueue',
    handler: (message, context) => {
        context.log('Service bus queue function processed message:', message);
    }
});
