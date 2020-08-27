import logging

# Add any dependencies you might need here
import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    """
    This is the function that will be called when calling your Azure Function.
    Currently, it is a template for an HTTP Trigger that says hello when calling your URL.
    Try changing the code found below to manipulate your function!
    If you want to change where your Azure Function looks, take a look at 'function.json'
    """
    logging.info('Python HTTP trigger function processed a request.')

    # Getting the 'name' parameter from the url string
    name = req.params.get('name')

    # Quick check to see if 'name' parameter exists from above
    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )
