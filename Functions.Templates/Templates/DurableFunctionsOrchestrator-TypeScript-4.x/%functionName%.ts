import { app, HttpHandler, HttpRequest, InvocationContext } from '@azure/functions';
import * as df from 'durable-functions';
import { ActivityHandler, OrchestrationHandler } from 'durable-functions';

const activityName = '%functionName%';

const %functionName%Orchestrator: OrchestrationHandler = function* (context) {
    const outputs = [];
    outputs.push(yield context.df.callActivity(activityName, 'Tokyo'));
    outputs.push(yield context.df.callActivity(activityName, 'Seattle'));
    outputs.push(yield context.df.callActivity(activityName, 'Cairo'));

    return outputs;
};
df.app.orchestration('%functionName%Orchestrator', %functionName%Orchestrator);

const %functionName%: ActivityHandler = (input: string) => {
    return `Hello, ${input}`;
};
df.app.activity(activityName, { handler: %functionName% });

const clientInput = df.input.durableClient();

const %functionName%HttpStart: HttpHandler = async (request: HttpRequest, context: InvocationContext) => {
    const client = df.getClient(context, clientInput);
    const body = await request.text();
    const instanceId = await client.startNew(request.params.orchestratorName, undefined, body);
    context.log(`Started orchestration with ID = '${instanceId}'.`);
    return client.createCheckStatusResponse(request, instanceId);
};

app.http('%functionName%HttpStart', {
    route: 'orchestrators/{orchestratorName}',
    extraInputs: [clientInput],
    handler: %functionName%HttpStart,
});