import { app, InvocationContext } from "@azure/functions";

export async function %functionName%(documents: unknown[], context: InvocationContext): Promise<void> {
    context.log(`Cosmos DB function processed ${documents.length} documents`);
}

app.cosmosDB('%functionName%', {
    connectionStringSetting: '%connectionStringSetting%',
    databaseName: '%databaseName%',
    collectionName: '%collectionName%',
    createLeaseCollectionIfNotExists: %createLeaseCollectionIfNotExists%,
    handler: %functionName%
});
