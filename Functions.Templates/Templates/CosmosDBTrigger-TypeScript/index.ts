import { AzureFunction, Context } from "@azure/functions"

const cosmosDBTrigger: AzureFunction = async function (context: Context, documents: any[]): Promise<void> {
    if (!!documents && documents.length > 0) {
        context.log('Document Id: ', documents[0].id);
    }
}

export default cosmosDBTrigger;
