const { app, HttpResponse } = require('@azure/functions');
const df = require('durable-functions');

const entityName = '$(FUNCTION_NAME_INPUT)';

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

app.http('$(FUNCTION_NAME_INPUT)HttpStart', {
    route: `${entityName}/{id}`,
    extraInputs: [df.input.durableClient()],
    handler: async (req, context) => {
        const id = req.params.id;
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
    },
});
