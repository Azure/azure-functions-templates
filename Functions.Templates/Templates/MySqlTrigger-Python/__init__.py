import azure.functions as func
import json
import logging

def main(changes: str) :
    """Sample MySql Trigger Binding

    *IMPORTANT*
        Local Development : You must have version >= 4.0.5030 of the Azure Function Core Tools installed.

    These tasks should be completed prior to running :
    1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
    2. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySql connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    4. Add 'az_func_updated_at' column in the table on which the changes are to be monitored.
    5. Add an app setting named "WEBSITE_SITE_NAME" containing the website name.

    Arguments:
    changes: The list of updated objects returned by the MySql trigger binding
    """
    if changes:
        logging.info("MySQL Changes: ")
    
    json_changes = json.loads(changes)
    for change in json_changes:
        product = func.MySqlRow(change["Item"]) 
        logging.info(product.data)
