import { app, HttpRequest, HttpResponse, InvocationContext } from "@azure/functions";

export async function %functionName%(request: HttpRequest, context: InvocationContext): Promise<HttpResponse> {
    context.log(`Http function processed request for url "${request.url}"`);

    const name = request.query.get('name') || await request.text() || 'world';

    return { body: `Hello, ${name}!` };
};

app.http('%functionName%', {
    methods: ['GET', 'POST'],
    authLevel: 'anonymous',
    handler: %functionName%
});
