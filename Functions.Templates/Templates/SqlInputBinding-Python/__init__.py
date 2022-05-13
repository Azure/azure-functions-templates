import azure.functions as func
import json

def main(req: func.HttpRequest, items: func.SqlRowList) -> func.HttpResponse:
    """Sample SQL Input Binding

    See https://aka.ms/sqlbindingsinput for more information about using this binding

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"

    Arguments:
    req: The HttpRequest that triggered this function
    items: The list of objects returned by the SQL input binding
    """

    rows = list(map(lambda r: json.loads(r.to_json()), items))

    return func.HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )