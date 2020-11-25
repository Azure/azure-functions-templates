import { AzureFunction, Context } from "@azure/functions"

const rabbitmqTrigger: AzureFunction = async function (context: Context, myQueueItem: string): Promise<void> {
    context.log('RabbitMQ trigger function processed work item', myQueueItem);
};

export default rabbitmqTrigger;
