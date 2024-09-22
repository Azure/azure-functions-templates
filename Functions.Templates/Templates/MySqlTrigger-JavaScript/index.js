/**
 * Sample MySql Trigger Binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "tableName" in function.json - this should be the table that is monitored for changes and triggers/invokes the function.
 *      2. Add an app setting named "MySqlConnectionString" containing the connection string
 *          to use for the MySql connection
 *      3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
 *		4. Add 'updated_at' column in the table on which the changes are to be monitored.
 * @param context The Azure Function runtime context
 * @param changes The updated objects returned by the trigger binding
 */
module.exports = async function (context, changes) {
    context.log(`MySql Changes: ${JSON.stringify(changes)}`)
}
