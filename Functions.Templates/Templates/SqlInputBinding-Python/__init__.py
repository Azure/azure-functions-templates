import json

from azure.functions import HttpRequest, HttpResponse, SqlRowList

def main(req: HttpRequest, items: SqlRowList) -> HttpResponse:
    """Sample SQL Input Binding

    See https://aka.ms/sqlbindingsinput for more information about using this binding

    *IMPORTANT*
        Local Development : You must have version >= 4.0.5030 of the Azure Function Core Tools installed.

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection

    Arguments:
    req: The HttpRequest that triggered this function
    items: The list of objects returned by the SQL input binding
    """

    rows = list(map(lambda r: json.loads(r.to_json()), items))

    return HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )
