import azure.functions as func
import azure.durable_functions as df

dfApp = df.DFApp(http_auth_level=func.AuthLevel.ANONYMOUS)

# Orchestrator
@dfApp.orchestration_trigger(context_name="context")
def $(FUNCTION_NAME_INPUT)(context):
    result1 = yield context.call_activity("hello", "Seattle")
    result2 = yield context.call_activity("hello", "Tokyo")
    result3 = yield context.call_activity("hello", "London")

    return [result1, result2, result3]