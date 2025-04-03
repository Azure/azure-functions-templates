# Activity
@$(BLUEPRINT_FILENAME).activity_trigger(input_name="city")
def $(FUNCTION_NAME_INPUT)_activity(city: str):
    return "Hello " + city 


# An HTTP-Triggered Function with a Durable Functions Client binding
@$(BLUEPRINT_FILENAME).route(route="orchestrators/{functionName}")
@$(BLUEPRINT_FILENAME).durable_client_input(client_name="client")
async def $(FUNCTION_NAME_INPUT)_starter(req: func.HttpRequest, client):
    function_name = req.route_params.get('functionName')
    instance_id = await client.start_new(function_name)
    response = client.create_check_status_response(req, instance_id)
    return response

# Orchestrator
@$(BLUEPRINT_FILENAME).orchestration_trigger(context_name="context")
def $(FUNCTION_NAME_INPUT)_orchestrator(context):
    result1 = yield context.call_activity("$(FUNCTION_NAME_INPUT)_activity", "Seattle")
    result2 = yield context.call_activity("$(FUNCTION_NAME_INPUT)_activity", "Tokyo")
    result3 = yield context.call_activity("$(FUNCTION_NAME_INPUT)_activity", "London")

    return [result1, result2, result3]
