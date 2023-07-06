const { app } = require('@azure/functions');

app.storageQueue('$(FUNCTION_NAME_INPUT)', {
    queueName: '$(QUEUE_NAME_INPUT)',
    connection: '$(CONNECTION_INPUT)',
    handler: (queueItem, context) => {
        context.log('Storage queue function processed work item:', queueItem);
    }
});
