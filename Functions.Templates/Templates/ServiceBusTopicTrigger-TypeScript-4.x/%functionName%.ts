import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(message: unknown, context: InvocationContext): Promise<void> {
    context.log('Service bus topic function processed message:', message);
}

app.serviceBusTopic('%functionName%', {
    connection: '%connection%',
    topicName: '%topicName%',
    subscriptionName: '%subscriptionName%',
    handler: %functionName%
});
