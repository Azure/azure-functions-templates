import { app, HttpHandler, HttpRequest, HttpResponse, InvocationContext } from '@azure/functions';
import * as df from 'durable-functions';
import { EntityContext, EntityHandler } from 'durable-functions';

const entityName = '%functionName%';

const %functionName%: EntityHandler<number> = (context: EntityContext<number>) => {
    const currentValue: number = context.df.getState(() => 0);
    switch (context.df.operationName) {
        case 'add':
            const amount: number = context.df.getInput();
            context.df.setState(currentValue + amount);
            break;
        case 'reset':
            context.df.setState(0);
            break;
        case 'get':
            context.df.return(currentValue);
            break;
    }
};
df.app.entity(entityName, %functionName%);

const %functionName%HttpStart: HttpHandler = async (req: HttpRequest, context: InvocationContext): Promise<HttpResponse> => {
    const id: string = req.params.id;
    const entityId = new df.EntityId(entityName, id);
    const client = df.getClient(context);

    if (req.method === 'POST') {
        // increment value
        await client.signalEntity(entityId, 'add', 1);
    } else {
        // read current state of entity
        const stateResponse = await client.readEntityState(entityId);
        return new HttpResponse({
            jsonBody: stateResponse.entityState,
        });
    }
};
app.http('%functionName%HttpStart', {
    route: `${entityName}/{id}`,
    extraInputs: [df.input.durableClient()],
    handler: %functionName%HttpStart,
});
