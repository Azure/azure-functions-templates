import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(message: unknown, context: InvocationContext): Promise<void> {
    context.log('Service bus queue function processed message:', message);
}

app.serviceBusQueue('%functionName%', {
    connection: '%connection%',
    queueName: '%queueName%',
    handler: %functionName%
});
