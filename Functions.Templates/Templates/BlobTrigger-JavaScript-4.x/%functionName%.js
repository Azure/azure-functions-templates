const { app } = require('@azure/functions');

app.storageBlob('%functionName%', {
    path: '%path%',
    connection: '%connection%',
    handler: (context, blob) => {
        context.log(`Storage blob function processed blob "${context.triggerMetadata.name}" with size ${blob.length} bytes`);
    }
});
