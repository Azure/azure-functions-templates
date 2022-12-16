const { app } = require('@azure/functions');

app.eventGrid('%functionName%', {
    handler: (event, context) => {
        context.log('Event grid function processed event:', event);
    }
});
