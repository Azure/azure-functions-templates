import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    const name = (req.query.name || (req.body && req.body.name));

    if (name) {
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "Hello " + (req.query.name || req.body.name) + ". The HTTP trigger function executed succefully"
        };
    }
    else {
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "The HTTP trigger function executed succefully. Pass a name in the query string or in the request body for a personalized response."
        };
    }
};

export default httpTrigger;
