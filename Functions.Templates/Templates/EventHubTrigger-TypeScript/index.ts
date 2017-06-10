export function run(context: any, eventHubMessages: any[]): void {
    context.log(`TypeScript eventhub trigger function called for message array ${eventHubMessages}`);

    eventHubMessages.forEach(message => {
        context.log(`Processed message ${message}`);
    });

    context.done();
};