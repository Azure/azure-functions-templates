const { app, HttpResponse } = require('@azure/functions');
const df = require('durable-functions');

const entityName = '%functionName%';

const clientInput = df.input.durableClient();

app.http('%functionName%HttpStart', {
    route: `${entityName}/{id}`,
    extraInputs: [clientInput],
    handler: async (req, context) => {
        const id = req.params.id;
        const entityId = new df.EntityId(entityName, id);
        const client = df.getClient(context, clientInput);

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
    },
});

df.app.entity(entityName, (context) => {
    const currentValue = context.df.getState(() => 0);
    switch (context.df.operationName) {
        case 'add':
            const amount = context.df.getInput();
            context.df.setState(currentValue + amount);
            break;
        case 'reset':
            context.df.setState(0);
            break;
        case 'get':
            context.df.return(currentValue);
            break;
    }
});