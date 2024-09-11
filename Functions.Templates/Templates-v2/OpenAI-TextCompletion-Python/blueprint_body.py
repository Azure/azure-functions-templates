@$(BLUEPRINT_FILENAME).route(route="$(FUNCTION_NAME_INPUT)", methods=["GET"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@$(BLUEPRINT_FILENAME).text_completion_input(arg_name="response", prompt="{Prompt}", max_tokens="100", model="$(CHAT_MODEL_NAME)")
def $(FUNCTION_NAME_INPUT)(req: func.HttpRequest, response: str) -> func.HttpResponse:
    response_json = json.loads(response)
    return func.HttpResponse(response_json["content"], status_code=200)
