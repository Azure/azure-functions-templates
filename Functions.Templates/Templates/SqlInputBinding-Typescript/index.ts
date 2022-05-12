import { AzureFunction, Context, HttpRequest } from "@azure/functions"

/**
 * Sample SQL Input Binding
 * See https://aka.ms/sqlbindingsinput for more information about using this binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
 *      2. Add an app setting named "SqlConnectionString" containing the connection string
 *          to use for the SQL connection
 *      3. Change the bundle name in host.json to "Microsoft.Azure.Functions.ExtensionBundle.Preview" and the version to "[3.*, 4.0.0)"
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 * @param items The array of objects returned by the SQL input binding
 */
const sqlInputBinding: AzureFunction = async function (context: Context, req: HttpRequest, items: any[]): Promise<void> {
    context.log('HTTP trigger with SQL input binding function processed a request.');
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: items
    };
};

export default sqlInputBinding;