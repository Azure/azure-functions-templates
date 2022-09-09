import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(context: InvocationContext, messages: unknown | unknown[]): Promise<void> {
    if (Array.isArray(messages)) {
        context.log(`Event hub function processed ${messages.length} messages`);
        for (const message of messages) {
            context.log('Event hub message:', message);
        }
    } else {
        context.log('Event hub function processed message:', messages);
    }
}

app.eventHub('%functionName%', {
    connection: '%connection%',
    eventHubName: '%eventHubName',
    cardinality: 'many',
    handler: %functionName%
});
