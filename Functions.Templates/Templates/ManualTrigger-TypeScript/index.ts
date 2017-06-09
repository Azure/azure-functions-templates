export function run(context: any, input: any) {
    context.log(`TypeScript manually triggered function called with input: ${input}`);
    context.done();
};