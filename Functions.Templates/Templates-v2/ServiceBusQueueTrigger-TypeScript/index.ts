import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(message: unknown, context: InvocationContext): Promise<void> {
    context.log('Service bus queue function processed message:', message);
}

app.serviceBusQueue('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    queueName: '$(QUEUE_NAME_INPUT)',
    handler: $(FUNCTION_NAME_INPUT)
});
