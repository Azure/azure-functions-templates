import { AzureFunction, Context, HttpRequest } from "@azure/functions"

/**
 * Sample MySql Output Binding
 *
 * These tasks should be completed prior to running :
 *      1. Update "commandText" in function.json - this should be the name of the table that you wish to upsert values to
 *      2. Add an app setting named "MySqlConnectionString" containing the connection string
 *          to use for the MySql connection
 * @param context The Azure Function runtime context
 * @param req The HttpRequest that triggered this function
 */
const mysqlOutputBinding: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger with MySql output binding function processed a request.');

    // Set items array to context.bindings for upsertion
    // Note that this expects the body to be a JSON object or array of objects which have a property
    // matching each of the columns in the table to upsert to.
    context.bindings.items = JSON.stringify(req.body);

    context.res = {
        status: 201, // 201 Created
        body: req.body
    };
};

export default mysqlOutputBinding;
