import { app, HttpHandler, HttpRequest, HttpResponse, InvocationContext } from '@azure/functions';
import * as df from 'durable-functions';
import { ActivityHandler, OrchestrationContext, OrchestrationHandler } from 'durable-functions';

const activityName = '$(FUNCTION_NAME_INPUT)';

const $(FUNCTION_NAME_INPUT)Orchestrator: OrchestrationHandler = function* (context: OrchestrationContext) {
    const outputs = [];
    outputs.push(yield context.df.callActivity(activityName, 'Tokyo'));
    outputs.push(yield context.df.callActivity(activityName, 'Seattle'));
    outputs.push(yield context.df.callActivity(activityName, 'Cairo'));

    return outputs;
};
df.app.orchestration('$(FUNCTION_NAME_INPUT)Orchestrator', $(FUNCTION_NAME_INPUT)Orchestrator);

const $(FUNCTION_NAME_INPUT): ActivityHandler = (input: string): string => {
    return `Hello, ${input}`;
};
df.app.activity(activityName, { handler: $(FUNCTION_NAME_INPUT) });

const $(FUNCTION_NAME_INPUT)HttpStart: HttpHandler = async (request: HttpRequest, context: InvocationContext): Promise<HttpResponse> => {
    const client = df.getClient(context);
    const body: unknown = await request.text();
    const instanceId: string = await client.startNew(request.params.orchestratorName, { input: body });

    context.log(`Started orchestration with ID = '${instanceId}'.`);

    return client.createCheckStatusResponse(request, instanceId);
};

app.http('$(FUNCTION_NAME_INPUT)HttpStart', {
    route: 'orchestrators/{orchestratorName}',
    extraInputs: [df.input.durableClient()],
    handler: $(FUNCTION_NAME_INPUT)HttpStart,
});