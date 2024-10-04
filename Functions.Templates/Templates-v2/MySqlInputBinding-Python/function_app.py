import json

from azure.functions import FunctionApp, HttpRequest, HttpResponse, MySqlRowList
 
app = FunctionApp()

@app.route(route="getitems")
@app.generic_input_binding(arg_name="items",
                           type="mysql",
                           commandText= "SELECT * FROM table1",
                           command_type="Text",
                           connection_string_setting="MySqlConnectionString")

def main(req: HttpRequest, items: MySqlRowList) -> HttpResponse:
    
    """Sample MySql Input Binding

    See https://aka.ms/mysqlbindingsintput for more information about using this binding
    
    These tasks should be completed prior to running :
    1. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySql connection
    2. Add an app setting named "PYTHON_ISOLATE_WORKER_DEPENDENCIES" : 1
    Arguments:
    req: The HttpRequest that triggered this function
    items: The list of objects returned by the MySql input binding
    """ 

    rows = list(map(lambda r: json.loads(r.to_json()), items))
    return HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )
