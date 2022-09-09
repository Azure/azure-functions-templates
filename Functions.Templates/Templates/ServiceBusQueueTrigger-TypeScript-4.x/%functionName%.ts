import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(context: InvocationContext, message: unknown): Promise<void> {
    context.log('Service bus queue function processed message:', message);
}

app.serviceBusQueue('%functionName%', {
    connection: '%connection%',
    queueName: '%queueName%',
    handler: %functionName%
});
