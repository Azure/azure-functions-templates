const { app } = require('@azure/functions');

app.http('%functionName%', {
    methods: ['GET', 'POST'],
    authLevel: 'anonymous',
    handler: async (context, request) => {
        context.log(`Http function processed request for url "${request.url}"`);

        const name = request.query.get('name') || await request.text() || 'world';

        return { body: `Hello, ${name}!` };
    }
});
