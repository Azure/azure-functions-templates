const { app } = require('@azure/functions');

app.serviceBusQueue('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    queueName: '$(QUEUE_NAME_INPUT)',
    handler: (message, context) => {
        context.log('Service bus queue function processed message:', message);
    }
});
