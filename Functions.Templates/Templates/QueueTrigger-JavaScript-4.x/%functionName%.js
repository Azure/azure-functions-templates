const { app } = require('@azure/functions');

app.storageQueue('%functionName%', {
    queueName: '%queueName%',
    connection: '%connection%',
    handler: (queueItem, context) => {
        context.log('Storage queue function processed work item:', queueItem);
    }
});
