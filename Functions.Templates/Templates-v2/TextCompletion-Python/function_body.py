@app.route(route="whois/{name}", methods=["GET"])
@app.text_completion_input(arg_name="response", prompt="Who is {name}?", max_tokens="100", model = "%CHAT_MODEL_DEPLOYMENT_NAME%")
def $(FUNCTION_NAME_INPUT)(req: func.HttpRequest, response: str) -> func.HttpResponse:
    response_json = json.loads(response)
    return func.HttpResponse(response_json["content"], status_code=200)
