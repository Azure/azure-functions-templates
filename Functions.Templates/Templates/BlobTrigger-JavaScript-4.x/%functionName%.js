const { app } = require('@azure/functions');

app.storageBlob('%functionName%', {
    path: 'samples-workitems/{name}',
    connection: '',
    handler: (blob, context) => {
        context.log(`Storage blob function processed blob "${context.triggerMetadata.name}" with size ${blob.length} bytes`);
    }
});
