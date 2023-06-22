/**
 * Kusto Output Binding
 * See https://github.com/Azure/Webjobs.Extensions.Kusto#output-binding for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "database" in function.json - this should be the name of the database where the table for inserting values resides
 *      2. Update "tableName" in function.json - this should be the name of the table that you wish to insert values to
 *      3. Add an app setting named "KustoConnectionString" containing the connection string
 *          to use for the Kusto connection
 *      4. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 */
// Upsert the product, which will insert it into the Products table if the primary key (ProductId) for that item doesn't exist.
// If it does then update it to have the new name and cost.
module.exports = async function (context, req) {
    // Note that this expects the body to be a JSON object or array of objects which have a property
    // matching each of the columns in the table to upsert to.
    context.bindings.product = req.body;

    return {
        status: 201,
        body: req.body
    };
}