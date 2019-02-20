import { AzureFunction, Context } from "@azure/functions"

const serviceBusTopicTrigger: AzureFunction = async function(context: Context, mySbMsg: any): Promise<void> {
    context.log('ServiceBus topic trigger function processed message', mySbMsg);
};

export default serviceBusTopicTrigger;
