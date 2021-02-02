import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    const message = (req.query.message || (req.body && req.body.message));
    const responseMessage = message
        ? "Message received: " + message + ". The message transfered to the kafka broker."
        : "This HTTP triggered function executed successfully. Pass a message in the query string or in the request body for a personalized response.";
    context.bindings.outputKafkaMessage = "Message : " + message;
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: responseMessage
    };

};

export default httpTrigger;