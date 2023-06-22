import { AzureFunction, Context } from "@azure/functions"

const queueTrigger: AzureFunction = async function (context: Context, myQueueItem: any): Promise<void> {
    context.log('Queue trigger function processed work item', myQueueItem);
};

export default queueTrigger;
