import json
import logging

def main(changes) :
    """Sample SQL Trigger Binding

    See https://aka.ms/sqlbindings for more information about using this binding

    *IMPORTANT*
        Local Development : You must have version >= 4.0.5030 of the Azure Function Core Tools installed.

    These tasks should be completed prior to running :
    1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
    2. Add an app setting named "SqlConnectionString" containing the connection string to use for the SQL connection
    3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"

    Arguments:
    changes: The list of updated objects returned by the SQL trigger binding
    """

    logging.info("SQL Changes: %s", json.loads(changes))
