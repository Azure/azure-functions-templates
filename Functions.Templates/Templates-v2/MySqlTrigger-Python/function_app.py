import json
import logging
import azure.functions as func

app = func.FunctionApp()

# The function gets triggered when a change (Insert, Update)
# is made to the Products table.
@app.function_name(name="ProductsTrigger")
@app.mysql_trigger(arg_name="products",
                        table_name="Products",
                        connection_string_setting="MySqlConnectionString")
 
def products_trigger(products: str) -> None:
    logging.info("MySQL Changes: %s", json.loads(products))

