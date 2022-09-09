const { app } = require('@azure/functions');

app.serviceBusQueue('%functionName%', {
    connection: '%connection%',
    queueName: '%queueName%',
    handler: (context, message) => {
        context.log('Service bus queue function processed message:', message);
    }
});
