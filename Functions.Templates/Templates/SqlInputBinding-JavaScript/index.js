/**
 * Sample SQL Input Binding
 * See https://aka.ms/sqlbindingsinput for more information about using this binding
 *
 * These values in function.json should be updated prior to running :
 *      commandText - This should be the query to execute to retrieve the data values to return
 *      connectionStringSetting - This should match the name of the app setting containing the connection string
 *          to use for the SQL connection (default is SqlConnectionString)
 * @param context The Azure Function runtime context
 * @param results The array of objects returned by the SQL input binding
 */
module.exports = async function (context, results) {
    context.log('Query results: ', JSON.stringify(results));
    return {
        status: 200,
        body: results
    };
}
