import { AzureFunction, Context } from "@azure/functions"

const eventHubTrigger: AzureFunction = async function (context: Context, eventHubMessages: any[]) {
    context.log(`Eventhub trigger function called for message array ${eventHubMessages}`);
    
    eventHubMessages.forEach((message, index) => {
        context.log(`Processed message ${message}`);
    });
};

export default eventHubTrigger;
