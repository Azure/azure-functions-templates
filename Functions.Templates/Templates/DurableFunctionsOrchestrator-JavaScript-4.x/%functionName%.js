const { app } = require('@azure/functions');
const df = require('durable-functions');

const activityName = '%functionName%';

df.app.orchestration('%functionName%Orchestrator', function* (context) {
    const outputs = [];
    outputs.push(yield context.df.callActivity(activityName, 'Tokyo'));
    outputs.push(yield context.df.callActivity(activityName, 'Seattle'));
    outputs.push(yield context.df.callActivity(activityName, 'Cairo'));

    return outputs;
});

df.app.activity(activityName, {
    handler: (input) => {
        return `Hello, ${input}`;
    },
});

const clientInput = df.input.durableClient();

app.http('%functionName%HttpStart', {
    route: 'orchestrators/{orchestratorName}',
    extraInputs: [clientInput],
    handler: async (request, context) => {
        const client = df.getClient(context, clientInput);
        const body = await request.text();
        const instanceId = await client.startNew(request.params.orchestratorName, undefined, body);
        context.log(`Started orchestration with ID = '${instanceId}'.`);
        return client.createCheckStatusResponse(request, instanceId);
    },
});