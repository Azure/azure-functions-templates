import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(queueItem: unknown, context: InvocationContext): Promise<void> {
    context.log('Storage queue function processed work item:', queueItem);
}

app.storageQueue('%functionName%', {
    queueName: '%queueName%',
    connection: '%connection%',
    handler: %functionName%
});
