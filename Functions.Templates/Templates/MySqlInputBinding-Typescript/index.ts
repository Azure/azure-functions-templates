import { AzureFunction, Context, HttpRequest } from "@azure/functions"

/**
 * Sample MySql Input Binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the query to execute to retrieve the values being returned
 *      2. Add an app setting named "MySqlConnectionString" containing the connection string
 *          to use for the MySql connection
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 * @param items The array of objects returned by the MySql input binding
 */
const mysqlInputBinding: AzureFunction = async function (context: Context, req: HttpRequest, items: any[]): Promise<void> {
    context.log('HTTP trigger with MySql input binding function processed a request.');
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: items
    };
};

export default mysqlInputBinding;