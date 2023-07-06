import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(message: unknown, context: InvocationContext): Promise<void> {
    context.log('Service bus topic function processed message:', message);
}

app.serviceBusTopic('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    topicName: '$(TOPIC_NAME_INPUT)',
    subscriptionName: '$(SUBSCRIPTION_NAME_INPUT)',
    handler: $(FUNCTION_NAME_INPUT)
});
