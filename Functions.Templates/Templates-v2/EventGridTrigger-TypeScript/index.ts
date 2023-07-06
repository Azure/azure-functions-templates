import { app, EventGridEvent, InvocationContext } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(event: EventGridEvent, context: InvocationContext): Promise<void> {
    context.log('Event grid function processed event:', event);
}

app.eventGrid('$(FUNCTION_NAME_INPUT)', {
    handler: $(FUNCTION_NAME_INPUT)
});
