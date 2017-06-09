export function run(context: any, myTimer: any): any {
    const timeStamp: string = new Date().toISOString();

    if(myTimer.isPastDue) {
        context.log(`TypeScript is running late!`);
    }
    context.log(`TypeScript timer trigger function ran! ${timeStamp}`);

    context.done();
};