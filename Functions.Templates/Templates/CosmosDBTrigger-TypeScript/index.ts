import { AzureFunction, Context } from "@azure/functions"

// This template uses an outdated version of the Azure Cosmos DB extension. Learn about migrating to the new extension at https://aka.ms/migrate-to-cosmos-extension-v4
const cosmosDBTrigger: AzureFunction = async function (context: Context, documents: any[]): Promise<void> {
    if (!!documents && documents.length > 0) {
        context.log('Document Id: ', documents[0].id);
    }
}

export default cosmosDBTrigger;
