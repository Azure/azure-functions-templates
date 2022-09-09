import { app, EventGridEvent, InvocationContext } from "@azure/functions";

export async function %functionName%(context: InvocationContext, event: EventGridEvent): Promise<void> {
    context.log('Event grid function processed event:', event);
}

app.eventGrid('%functionName%', {
    handler: %functionName%
});
