import azure.functions as func
import logging
import json

app = func.FunctionApp()

@app.mysql_db_trigger(arg_name="azmysqlchangeslist", table_name="$(TABLE_NAME_INPUT)",
                        connection="$(CONNECTION_STRING_INPUT)")  
def $(FUNCTION_NAME_INPUT)(azmysqlchangeslist: str):
    logging.info("MySQL Changes: %s", json.loads(azmysqlchangeslist))
