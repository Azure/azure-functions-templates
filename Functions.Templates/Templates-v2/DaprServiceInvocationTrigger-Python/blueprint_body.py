
@$(BLUEPRINT_FILENAME).dapr_service_invocation_trigger(arg_name="payload", method_name="$(FUNCTION_NAME_INPUT)") 
def $(FUNCTION_NAME_INPUT)(payload: str) :
    logging.info('Azure function triggered by Dapr Service Invocation Trigger.')
    logging.info("Dapr service invocation trigger payload: %s", payload)

@dapp.function_name(name="InvokeOutputBinding")
@dapp.route(route="invoke/{appId}/{methodName}", auth_level=dapp.auth_level.ANONYMOUS)
@dapp.dapr_invoke_output(arg_name = "payload", app_id = "{appId}", method_name = "{methodName}", http_verb = "post")
def main(req: func.HttpRequest, payload: func.Out[str] ) -> str:
    logging.info('Python HTTP trigger function processed a request..')

    body = req.get_body()
    payload.set(body)

    return 'Successfully performed service invocation using Dapr invoke output binding.'