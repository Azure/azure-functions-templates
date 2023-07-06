const { app } = require('@azure/functions');

app.cosmosDB('$(FUNCTION_NAME_INPUT)', {
    connection: '$(CONNECTION_INPUT)',
    databaseName: '$(DATABASE_NAME_INPUT)',
    containerName: '$(CONTAINER_NAME_INPUT)',
    createLeaseContainerIfNotExists: $(CREATE_LEASE_CONTAINER_IF_NOT_EXISTS_INPUT),
    handler: (documents, context) => {
        context.log(`Cosmos DB function processed ${documents.length} documents`);
    }
});
