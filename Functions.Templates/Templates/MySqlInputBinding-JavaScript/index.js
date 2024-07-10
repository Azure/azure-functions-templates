/**
 * Sample MySQL Input Binding
 * See https://aka.ms/sqlbindingsinput for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
 *      2. Add an app setting named "MySqlConnectionString" containing the connection string
 *          to use for the MySql connection
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 * @param results The array of objects returned by the MySql input binding
 */
module.exports = async function (context, req, results) {
    context.log('Query results: ', JSON.stringify(results));
    return {
        status: 200,
        body: results
    };
}
