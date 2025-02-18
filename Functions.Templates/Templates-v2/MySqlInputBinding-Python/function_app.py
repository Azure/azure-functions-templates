import json
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="GetProducts")
@app.route(route="getproducts/{cost}")
@app.mysql_input(arg_name="product",
                        command_text="select * from Products where Cost = @Cost",
                        command_type="Text",
                        parameters="@Cost={cost}",
                        connection_string_setting="MySqlConnectionString")

def getproducts(req: func.HttpRequest, product: func.MySqlRowList) -> func.HttpResponse:
    rows = list(map(lambda r: json.loads(r.to_json()), product))

    return func.HttpResponse(
        json.dumps(rows),
        status_code=200,
        mimetype="application/json"
    )


