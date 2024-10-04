import json
import logging

from azure.functions import FunctionApp
 
app = FunctionApp()

@app.generic_trigger(arg_name="items",
                        type="mysqlTrigger",
                        table_name="table1",
                        connection_string_setting="MySqlConnectionString")

def main(items: str) -> None:
        
    """Sample MySql Trigger Binding

    These tasks should be completed prior to running :
    1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
    2. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySql connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    4. Add 'az_func_updated_at' column in the table on which the changes are to be monitored.
    5. Add an app setting named "PYTHON_ISOLATE_WORKER_DEPENDENCIES" : 1

    Arguments:
    changes: The list of updated objects returned by the MySql trigger binding
    """

    logging.info("MySQL Changes: %s", json.loads(items))
