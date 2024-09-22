import json
import logging

def main(changes) :
    """Sample MySql Trigger Binding

    *IMPORTANT*
        Local Development : You must have version >= 4.0.5030 of the Azure Function Core Tools installed.

    These tasks should be completed prior to running :
    1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
    2. Add an app setting named "MySqlConnectionString" containing the connection string to use for the MySql connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
    4. Add 'updated_at' column in the table on which the changes are to be monitored.

    Arguments:
    changes: The list of updated objects returned by the MySql trigger binding
    """

    logging.info("MySql Changes: %s", json.loads(changes))
