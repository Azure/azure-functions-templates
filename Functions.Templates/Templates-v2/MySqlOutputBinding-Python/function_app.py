import json
import azure.functions as func

app = func.FunctionApp()

@app.function_name(name="AddProduct")
@app.route(route="addproduct")
@app.mysql_output(arg_name="product",
                           command_text= "Products",
                           command_type="Text",
                           connection_string_setting="MySqlConnectionString")
 
def add_product(req: func.HttpRequest, product: func.Out[func.MySqlRow]) -> func.HttpResponse:
    body = json.loads(req.get_body())
    row = func.MySqlRow.from_dict(body)
    product.set(row)
 
    return func.HttpResponse(
        body=req.get_body(),
        status_code=201,
        mimetype="application/json"
    )
