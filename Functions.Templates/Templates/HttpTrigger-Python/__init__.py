import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Python HTTP trigger function processed a request.")

    try:
        # Get value for `name` from parameters or JSON body
        name = req.params.get("name") or req.get_json().get("name")
    except ValueError:
        pass

    return (
        func.HttpResponse(f"Hello {name}!")
        if name
        else func.HttpResponse(
            "Please pass a name on the query string or in the request body",
            status_code=400,
        )
    )
