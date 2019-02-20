import { AzureFunction, Context } from "@azure/functions"

const eventGridTrigger: AzureFunction = async function (context: Context, eventGridEvent: any): Promise<void> {
    context.log(typeof eventGridEvent);
    context.log(eventGridEvent);
};

export default eventGridTrigger;
