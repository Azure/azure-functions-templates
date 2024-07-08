
@$(BLUEPRINT_FILENAME).route(route="injest_file", methods=["POST"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@$(BLUEPRINT_FILENAME).embeddings_store_output(arg_name="requests", input="{url}", input_type="url", connection_name="$(CONNECTION_STRING_INPUT)", collection="$(COLLECTION_NAME)", model="$(EMBEDDING_MODEL_NAME)")
def ingest_file(req: func.HttpRequest, requests: func.Out[str]) -> func.HttpResponse:
    import json

    user_message = req.get_json()
    if not user_message:
        return func.HttpResponse(json.dumps({"message": "No message provided"}), status_code=400, mimetype="application/json")
    file_name_with_extension = os.path.basename(user_message["Url"])
    title = os.path.splitext(file_name_with_extension)[0]
    create_request = {  
        "title": title
    }
    requests.set(json.dumps(create_request))
    response_json = {
        "status": "success",
        "title": title
    }
    return func.HttpResponse(json.dumps(response_json), status_code=200, mimetype="application/json")


@$(BLUEPRINT_FILENAME).route(route="prompt_file", methods=["POST"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@$(BLUEPRINT_FILENAME).semantic_search_input(arg_name="result", connection_name="$(CONNECTION_STRING_INPUT)", collection="$(COLLECTION_NAME)", query="{Prompt}", embeddings_model="$(EMBEDDING_MODEL_NAME)", chat_model="$(CHAT_MODEL_NAME)")
def prompt_file(req: func.HttpRequest, result: str) -> func.HttpResponse:
    import json 
    result_json = json.loads(result)
    response_json = {
        "content": result_json.get("Response"),
        "content_type": "text/plain"
    }
    return func.HttpResponse(json.dumps(response_json), status_code=200, mimetype="application/json")
