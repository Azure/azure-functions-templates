import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(context: InvocationContext, message: unknown): Promise<void> {
    context.log('Service bus topic function processed message:', message);
}

app.serviceBusTopic('%functionName%', {
    connection: '%connection%',
    topicName: '%topicName%',
    subscriptionName: '%subscriptionName%',
    handler: %functionName%
});
