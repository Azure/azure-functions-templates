const { app } = require('@azure/functions');

app.timer('$(FUNCTION_NAME_INPUT)', {
    schedule: '$(SCHEDULE_INPUT)',
    handler: (myTimer, context) => {
        context.log('Timer function processed request.');
    }
});
