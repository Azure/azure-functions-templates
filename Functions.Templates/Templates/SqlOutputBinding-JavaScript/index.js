/**
 * Sample SQL Output Binding
 * See https://aka.ms/sqlbindingsoutput for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
 *      2. Add an app setting named "SqlConnectionString" containing the connection string
 *          to use for the SQL connection
 *      3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 */
module.exports = async function (context, req) {
    context.log('HTTP trigger with SQL output binding function processed a request.');

    // Set results array to context.bindings for upsertion
    // Note that this expects the body to be a JSON object or array of objects which have a property
    // matching each of the columns in the table to upsert to.
    context.bindings.results = JSON.stringify(req.body);

    context.res = {
        status: 201,
        body: req.body
    };
}