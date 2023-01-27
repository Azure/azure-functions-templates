import { app, InvocationContext, Timer } from "@azure/functions";

export async function %functionName%(myTimer: Timer, context: InvocationContext): Promise<void> {
    context.log('Timer function processed request.');
}

app.timer('%functionName%', {
    schedule: '0 */5 * * * *',
    handler: %functionName%
});
