import logging

import azure.functions as func
# Add any dependencies you might want here and in requirements.txt

def main(req: func.HttpRequest) -> func.HttpResponse:
    """
    This is the function that will be called when calling your Azure Function.
    Currently, it is a template for an HTTP Trigger that says hello when calling your URL with a parameter 'name'
    Try changing the code and run it locally in VS Code by pressing F5!
    """
    logging.info('Python HTTP trigger function processed a request.')
    
    # HTTP GET request using url parameter name 'url?name=Azure'
    name = req.params.get('name')
    
    # HTTP POST request using json body with key 'name' and it's value
    if not name:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('name')

    # If the program finds 'name' through the URL or as JSON body, display it!
    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
