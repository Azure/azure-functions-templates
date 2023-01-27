const { app } = require('@azure/functions');

app.storageQueue('%functionName%', {
    queueName: 'js-queue-items',
    connection: '',
    handler: (queueItem, context) => {
        context.log('Storage queue function processed work item:', queueItem);
    }
});
