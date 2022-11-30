/**
 * Sample SQL Input Binding
 * See https://aka.ms/sqlbindingsinput for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
 *      2. Add an app setting named "SqlConnectionString" containing the connection string
 *          to use for the SQL connection
 *      3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[4.*, 5.0.0)"
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 * @param results The array of objects returned by the SQL input binding
 */
module.exports = async function (context, req, results) {
    context.log('Query results: ', JSON.stringify(results));
    return {
        status: 200,
        body: results
    };
}
