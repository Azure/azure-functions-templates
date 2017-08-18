export function run(context: any, IoTHubMessages: any[]): void {
    context.log(`TypeScript eventhub trigger function called for message array ${IoTHubMessages}`);

    IoTHubMessages.forEach(message => {
        context.log(`Processed message ${message}`);
    });

    context.done();
};