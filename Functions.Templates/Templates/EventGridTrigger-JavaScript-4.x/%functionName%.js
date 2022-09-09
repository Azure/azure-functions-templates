const { app } = require('@azure/functions');

app.eventGrid('%functionName%', {
    handler: (context, event) => {
        context.log('Event grid function processed event:', event);
    }
});
