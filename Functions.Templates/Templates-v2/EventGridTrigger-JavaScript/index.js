const { app } = require('@azure/functions');

app.eventGrid('$(FUNCTION_NAME_INPUT)', {
    handler: (event, context) => {
        context.log('Event grid function processed event:', event);
    }
});
