const { app } = require('@azure/functions');

app.cosmosDB('%functionName%', {
    connectionStringSetting: '%connectionStringSetting%',
    databaseName: '%databaseName%',
    collectionName: '%collectionName%',
    createLeaseCollectionIfNotExists: %createLeaseCollectionIfNotExists%,
    handler: (documents, context) => {
        context.log(`Cosmos DB function processed ${documents.length} documents`);
    }
});
