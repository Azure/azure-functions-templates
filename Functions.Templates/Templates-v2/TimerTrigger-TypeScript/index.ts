import { app, InvocationContext, Timer } from "@azure/functions";

export async function $(FUNCTION_NAME_INPUT)(myTimer: Timer, context: InvocationContext): Promise<void> {
    context.log('Timer function processed request.');
}

app.timer('$(FUNCTION_NAME_INPUT)', {
    schedule: '$(SCHEDULE_INPUT)',
    handler: $(FUNCTION_NAME_INPUT)
});
