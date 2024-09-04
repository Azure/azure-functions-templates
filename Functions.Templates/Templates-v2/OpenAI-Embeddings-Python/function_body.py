@app.route(route="$(FUNCTION_NAME_INPUT)", methods=["POST"], auth_level=func.AuthLevel.$(AUTHLEVEL_INPUT))
@app.embeddings_input(arg_name="embeddings", input="{rawText}", input_type="rawText", model="$(EMBEDDING_MODEL_NAME)")
def $(FUNCTION_NAME_INPUT)(req: func.HttpRequest, embeddings: str) -> func.HttpResponse:
    user_message = req.get_json()
    embeddings_json = json.loads(embeddings)
    embeddings_request = {
        "raw_text": user_message.get("RawText"),
        "file_path": user_message.get("FilePath")
    }
    logging.info(f'Received {embeddings_json.get("count")} embedding(s) for input text '
        f'containing {len(embeddings_request.get("raw_text"))} characters.')
    return func.HttpResponse(status_code=200)