export function run(context: any, item: any) {
    context.log(`TypeScript queue trigger function processed work item: ${item}`);
    context.done();
};