import azure.functions as func
import json

def main(req: func.HttpRequest, rowList: func.SqlRowList) -> func.HttpResponse:
    """Sample SQL Input Binding

    See https://aka.ms/sqlbindingsinput for more information about using this binding

    Arguments:
    req: The HttpRequest that triggered this function
    rowList: The list of objects returned by the SQL input binding
    """

    rows = list(map(lambda r: json.loads(r.to_json()), rowList))

    return func.HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )