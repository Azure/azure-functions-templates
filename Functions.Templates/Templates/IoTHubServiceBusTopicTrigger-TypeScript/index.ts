export function run(context: any, message: any) {
    context.log(`TypeScript ServiceBus topic trigger function processed message ${message}`);
    context.done();
};