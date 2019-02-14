import { AzureFunction, Context } from "@azure/functions"

const serviceBusQueueTrigger: AzureFunction = async function(context: Context, mySbMsg) {
    context.log('ServiceBus queue trigger function processed message', mySbMsg);
};

export default serviceBusQueueTrigger;
