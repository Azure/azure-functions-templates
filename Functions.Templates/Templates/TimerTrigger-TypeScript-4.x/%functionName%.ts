import { app, InvocationContext, Timer } from "@azure/functions";

export async function %functionName%(context: InvocationContext, myTimer: Timer): Promise<void> {
    context.log('Timer function processed request.');
}

app.timer('%functionName%', {
    schedule: '%schedule%',
    handler: %functionName%
});
