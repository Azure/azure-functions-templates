import { AzureFunction, Context } from "@azure/functions"

const queueTrigger: AzureFunction = async function (context: Context, myQueueItem: string) {
    context.log('JavaScript queue trigger function processed work item', myQueueItem);
};

export default queueTrigger;
