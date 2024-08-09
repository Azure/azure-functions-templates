import json

from azure.functions import HttpRequest, HttpResponse, Out, MySqlRowList, MySqlRow

def main(req: HttpRequest, items: Out[MySqlRowList]) -> HttpResponse:
    """Sample MySQL Output Binding

    See https://aka.ms/mysqlbindingsoutput for more information about using this binding

    *IMPORTANT*
        Local Development : You must have version >= 4.0.5030 of the Azure Function Core Tools installed.

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
    2. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySQL connection

    Arguments:
    req: The HttpRequest that triggered this function
    items: The objects to be upserted to the database
    """

    # Note that this expects the body to be an array of JSON objects which
    # have a property matching each of the columns in the table to upsert to.
    body = json.loads(req.get_body())
    rows = MySqlRowList(map(lambda r: MySqlRow.from_dict(r), body))
    items.set(rows)

    return HttpResponse(
        body=req.get_body(),
        status_code=201, # 201 Created
        mimetype="application/json"
    )
