import json
import azure.functions as func

def main(req: func.HttpRequest, product: func.Out[func.SqlRow]) -> func.HttpResponse:
    """Sample SQL Output Binding

    See https://aka.ms/sqlbindingsoutput for more information about using this binding

    Arguments:
    req: The HttpRequest that triggered this function
    product: The object to be upserted to the database
    """

    body = json.loads(req.get_body())
    product.set(func.SqlRow.from_dict(body))

    return func.HttpResponse(
        body,
        status_code=201,
        mimetype="application/json"
    )
