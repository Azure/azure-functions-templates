import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(blob: Buffer, context: InvocationContext): Promise<void> {
    context.log(`Storage blob function processed blob "${context.triggerMetadata.name}" with size ${blob.length} bytes`);
}

app.storageBlob('$(FUNCTION_NAME_INPUT)', {
    path: '$(PATH_INPUT)',
    connection: '$(CONNECTION_INPUT)',
    handler: $(FUNCTION_NAME_INPUT)
});
