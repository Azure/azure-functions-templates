// This template uses an outdated version of the Azure Cosmos DB extension. Learn about migrating to the new extension at https://aka.ms/migrate-to-cosmos-extension-v4
module.exports = async function (context, documents) {
    if (!!documents && documents.length > 0) {
        context.log('Document Id: ', documents[0].id);
    }
}
