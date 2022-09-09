const { app } = require('@azure/functions');

app.timer('%functionName%', {
    schedule: '%schedule%',
    handler: (context, myTimer) => {
        context.log('Timer function processed request.');
    }
});
