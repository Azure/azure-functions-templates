import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    # Parse the request to the HTTP endpoint for a value corresponding to `name`.
    # Return "Hello {name}!" if found or ask user for input.

    logging.info("Python HTTP trigger function processed a request.")

    # Get the value for `name` from URL arguments. `None` if not present.
    name = req.params.get("name")

    # If `name` is not found in the URL arguments, check the JSON body if it exists.
    if not name and hasattr(req, "get_json"):
        name = req.get_json().get("name")

    if name:
        # Greet the user if `name` is not `None`.
        return func.HttpResponse(f"Hello {name}!")
    else:
        # Ask the user for input.
        return func.HttpResponse(
            "Please pass a name on the query string or in the request body",
            status_code=400,
        )
