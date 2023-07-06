import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(messages: unknown | unknown[], context: InvocationContext): Promise<void> {
    if (Array.isArray(messages)) {
        context.log(`Event hub function processed ${messages.length} messages`);
        for (const message of messages) {
            context.log('Event hub message:', message);
        }
    } else {
        context.log('Event hub function processed message:', messages);
    }
}

app.eventHub('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    eventHubName: '$(EVENT_HUB_NAME_INPUT)',
    cardinality: 'many',
    handler: $(FUNCTION_NAME_INPUT)
});
