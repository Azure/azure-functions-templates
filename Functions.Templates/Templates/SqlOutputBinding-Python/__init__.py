import json
import azure.functions as func

def main(req: func.HttpRequest, items: func.Out[func.SqlRowList]) -> func.HttpResponse:
    """Sample SQL Output Binding

    See https://aka.ms/sqlbindingsoutput for more information about using this binding

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"

    Arguments:
    req: The HttpRequest that triggered this function
    items: The objects to be upserted to the database
    """

    # Note that this expects the body to be an array of JSON objects which
    # have a property matching each of the columns in the table to upsert to.
    body = json.loads(req.get_body())
    rows = list(map(lambda r: func.SqlRow.from_dict(r), body))
    items.set(rows)

    return func.HttpResponse(
        body=req.get_body(),
        status_code=201, # 201 Created
        mimetype="application/json"
    )
