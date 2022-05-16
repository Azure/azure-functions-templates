import azure.functions as func
import json

def main(req: func.HttpRequest, items: func.SqlRowList) -> func.HttpResponse:
    """Sample SQL Input Binding

    See https://aka.ms/sqlbindingsinput for more information about using this binding

    *IMPORTANT*
        Local Development : You must have v4.x of the Azure Function Core Tools installed, support for earlier versions will be added in a later release.
        Deployed App : The app must be deployed to the EUAP region, support for other regions will be added later.

    See https://github.com/Azure/azure-functions-sql-extension/issues/250 for the current state of Python support for the SQL binding

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    4. Update requirements.txt and change the "azure-functions" line to "azure-functions==1.11.3b1" *IMPORTANT* Support for durable functions is not available in this release. A future release will combine SQL bindings and durable functions capabilities.
    5. Add an app setting named "PYTHON_ISOLATE_WORKER_DEPENDENCIES" and set the value to "1" (to ensure that the correct version of the azure-functions library is used)

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
