import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(queueItem: unknown, context: InvocationContext): Promise<void> {
    context.log('Storage queue function processed work item:', queueItem);
}

app.storageQueue('$(FUNCTION_NAME_INPUT)', {
    queueName: '$(QUEUE_NAME_INPUT)',
    connection: '$(CONNECTION_INPUT)',
    handler: $(FUNCTION_NAME_INPUT)
});
