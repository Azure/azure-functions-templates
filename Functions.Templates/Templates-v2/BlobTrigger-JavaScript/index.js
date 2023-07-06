const { app } = require('@azure/functions');

app.storageBlob('$(FUNCTION_NAME_INPUT)', {
    path: '$(PATH_INPUT)',
    connection: '$(CONNECTION_INPUT)',
    handler: (blob, context) => {
        context.log(`Storage blob function processed blob "${context.triggerMetadata.name}" with size ${blob.length} bytes`);
    }
});
