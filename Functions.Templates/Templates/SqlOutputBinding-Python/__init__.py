import json
import azure.functions as func

def main(req: func.HttpRequest, items: func.Out[func.SqlRowList]) -> func.HttpResponse:
    """Sample SQL Output Binding

    See https://aka.ms/sqlbindingsoutput for more information about using this binding

    *IMPORTANT*
        Local Development : You must have v4.x of the Azure Function Core Tools installed, support for earlier versions will be added in a later release.
        Deployed App : The app must be deployed to the EUAP region, support for other regions will be added later.

    See https://github.com/Azure/azure-functions-sql-extension/issues/250 for the current state of Python support for the SQL binding

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"
    4. Update requirements.txt and change the "azure-functions" line to "azure-functions==1.11.3b1" *IMPORTANT* Support for durable functions is not available in this release. A future release will combine SQL bindings and durable functions capabilities.
    5. Add an app setting named "PYTHON_ISOLATE_WORKER_DEPENDENCIES" and set the value to "1" (to ensure that the correct version of the azure-functions library is used)

    Arguments:
    req: The HttpRequest that triggered this function
    items: The objects to be upserted to the database
    """

    # Note that this expects the body to be an array of JSON objects which
    # have a property matching each of the columns in the table to upsert to.
    body = json.loads(req.get_body())
    rows = func.SqlRowList(map(lambda r: func.SqlRow.from_dict(r), body))
    items.set(rows)

    return func.HttpResponse(
        body=req.get_body(),
        status_code=201, # 201 Created
        mimetype="application/json"
    )
