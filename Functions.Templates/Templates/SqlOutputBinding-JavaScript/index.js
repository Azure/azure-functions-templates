/**
 * Sample SQL Output Binding
 * See https://aka.ms/sqlbindingsoutput for more information about using this binding
 *
 * These values in function.json should be updated prior to running :
 *      commandText - This should be the name of the table that you wish to upsert values to
 *      connectionStringSetting - This should match the name of the app setting containing the connection string
 *          to use for the SQL connection (default is SqlConnectionString)
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 */
module.exports = async function (context, req) {
    context.log('HTTP trigger with SQL output binding function processed a request.');

    // Set array to context.bindings for insertion
    context.bindings.results = JSON.stringify(req.body);

    context.res = {
        status: 201,
        body: req.body
    };
}