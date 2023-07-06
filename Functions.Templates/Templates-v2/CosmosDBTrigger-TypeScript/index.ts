import { app, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(documents: unknown[], context: InvocationContext): Promise<void> {
    context.log(`Cosmos DB function processed ${documents.length} documents`);
}

app.cosmosDB('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    databaseName: '$(DATABASE_NAME_INPUT)',
    containerName: '$(CONTAINER_NAME_INPUT)',
    createLeaseContainerIfNotExists: $(CREATE_LEASE_CONTAINER_IF_NOT_EXISTS_INPUT),
    handler: $(FUNCTION_NAME_INPUT)
});
