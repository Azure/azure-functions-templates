import azure.functions as func

def main(req: func.HttpRequest, products: func.Out[str]) -> func.HttpResponse:
    """Kusto Output Binding
  See https://github.com/Azure/Webjobs.Extensions.Kusto#output-binding for more information about using this binding
 
  These tasks should be completed prior to running :
       1. Update "database" in function.json - this should be the name of the database where the table for inserting values resides
       2. Update "tableName" in function.json - this should be the name of the table that you wish to insert values to
       3. Add an app setting named "KustoConnectionString" containing the connection string
           to use for the Kusto connection
       4. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    Arguments:
    req: The HttpRequest that triggered this function
    items: The objects to be upserted to the database
    """
    # Note that this expects the body to be an array of JSON objects which
    # have a property matching each of the columns in the table to upsert to.
    body = str(req.get_body(),'UTF-8')
    products.set(body)
    return func.HttpResponse(
        body=body,
        status_code=201,
        mimetype="application/json"
    )
