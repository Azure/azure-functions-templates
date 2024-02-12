
# Orchestrator
@dfApp.orchestration_trigger(context_name="context")
def $(FUNCTION_NAME_INPUT)(context):
    result1 = yield context.call_activity("hello", "Seattle")
    result2 = yield context.call_activity("hello", "Tokyo")
    result3 = yield context.call_activity("hello", "London")

    return [result1, result2, result3]