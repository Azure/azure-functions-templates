import json

from azure.functions import FunctionApp, HttpRequest, HttpResponse, MySqlRowList, Out
 
app = FunctionApp()

@app.route(route="additems")
@app.generic_output_binding(arg_name="items",
                           type="mysql",
                           commandText= "itemss",
                           command_type="Text",
                           connection_string_setting="MySqlConnectionString")

def main(req: HttpRequest, items: Out[MySqlRowList]) -> HttpResponse:

    """Sample MySQL Output Binding

    See https://aka.ms/mysqlbindingsoutput for more information about using this binding

    These tasks should be completed prior to running :
    1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
    2. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySql connection
    3. Add an app setting named "PYTHON_ISOLATE_WORKER_DEPENDENCIES" : 1

    Arguments:
    req: The HttpRequest that triggered this function
    items: The objects to be upserted to the database
    """

    rows = list(map(lambda r: json.loads(r.to_json()), items))
    return HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )
