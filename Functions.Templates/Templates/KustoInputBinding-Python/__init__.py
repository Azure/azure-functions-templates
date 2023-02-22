import azure.functions as func
import json

def main(req: func.HttpRequest, products: str) -> func.HttpResponse:
    """Sample Kusto Input Binding
    See https://github.com/Azure/Webjobs.Extensions.Kusto#input-binding for more information about using this binding
 
    These tasks should be completed prior to running :
       1. Update "kqlCommand" in function.json - this should be a KQLQuery or Function being used for the query
       2. Update "database" in function.json - this should be the database the query has to be run against
       3. Optionally update "kqlParameters" in function.json - this should be the runtime predicates that can be used for the kqlCommand
       4. Add an app setting named "KustoConnectionString" containing the connection string
           to use for the Kusto connection
       5. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Arguments:
    req: The HttpRequest that triggered this function
    items: The list of objects returned by the Kusto query input binding
    """

    return func.HttpResponse(
        products,
        status_code=200,
        mimetype="application/json"
    )
