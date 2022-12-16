const { app } = require('@azure/functions');

app.timer('%functionName%', {
    schedule: '%schedule%',
    handler: (myTimer, context) => {
        context.log('Timer function processed request.');
    }
});
