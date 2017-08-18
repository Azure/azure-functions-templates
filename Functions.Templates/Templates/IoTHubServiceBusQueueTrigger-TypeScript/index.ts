export function run(context: any, mySbMsg: any): void {
    context.log(`TypeScript ServiceBus queue trigger function processed message: ${mySbMsg}`);
    context.done();
};