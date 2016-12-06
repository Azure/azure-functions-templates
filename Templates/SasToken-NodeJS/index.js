// An HTTP trigger Azure Function that returns a SAS token for Azure Storage for the specified container. 
// You can also optionally specify a particular blob name and access permissions. 
// To learn more, see https://github.com/Azure/azure-webjobs-sdk-templates/blob/master/Templates/SasToken-JavaScript/readme.md

var azure = require('azure-storage');

// Setup: npm install 
// Go to Function app settings -> App Service settings -> Tools -> Console and type the following:
//    > cd <functionName>
//    > npm install
// Or, go to http://yoursite.scm.azurewebsites.net/DebugConsole navigate to site/wwwroot/YourFunctionName
// and do npm install in the console window

module.exports = function(context, req) {
    if (req.body.container) {
        // The following values can be used for permissions: 
        // "a" (Add), "r" (Read), "w" (Write), "d" (Delete), "l" (List)
        // Concatenate multiple permissions, such as "rwa" = Read, Write, Add
        context.res = generateSasToken(context, req.body.container, req.body.blobName, req.body.permissions);
    } else {
        context.res = {
            status: 400,
            body: "Specify a value for 'container'"
        };
    }
    
    context.done();
};

function generateSasToken(context, container, blobName, permissions) {
    var connString = process.env.AzureWebJobsStorage;
    var blobService = azure.createBlobService(connString);

    // Create a SAS token that expires in an hour
    // Set start time to five minutes ago to avoid clock skew.
    var startDate = new Date();
    startDate.setMinutes(startDate.getMinutes() - 5);
    var expiryDate = new Date(startDate);
    expiryDate.setMinutes(startDate.getMinutes() + 60);

    permissions = permissions || azure.BlobUtilities.SharedAccessPermissions.READ;

    var sharedAccessPolicy = {
        AccessPolicy: {
            Permissions: permissions,
            Start: startDate,
            Expiry: expiryDate
        }
    };
    
    var sasToken = blobService.generateSharedAccessSignature(container, blobName, sharedAccessPolicy);
    
    return {
        token: sasToken,
        uri: blobService.getUrl(container, blobName, sasToken, true)
    };
}
