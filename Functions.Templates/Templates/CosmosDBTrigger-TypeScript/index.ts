import { AzureFunction, Context } from "@azure/functions"

const cosmosDBTrigger: AzureFunction = async function (context: Context, documents) {
    if (!!documents && documents.length > 0) {
        context.log('Document Id: ', documents[0].id);
    }
}

export default cosmosDBTrigger;
