import azure.functions as func
import azure.durable_functions as df

dfApp = df.DFApp(http_auth_level=func.AuthLevel.ANONYMOUS)

# An HTTP-Triggered Function with a Durable Functions Client binding
@dfApp.route(route="orchestrators/{functionName}")
@dfApp.durable_client_input(client_name="client")
async def $(FUNCTION_NAME_INPUT)(req: func.HttpRequest, client):
    function_name = req.route_params.get('functionName')
    instance_id = await client.start_new(function_name)
    response = client.create_check_status_response(req, instance_id)
    return response