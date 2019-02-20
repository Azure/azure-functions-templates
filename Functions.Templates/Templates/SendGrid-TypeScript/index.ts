import { AzureFunction, Context } from "@azure/functions"

// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }

const sendGrid: AzureFunction =  async function (context: Context, order: any): Promise<void> {
    context.log('Queue trigger function processed order', order.orderId);

    context.bindings.message = {
        subject: `Thanks for your order (#${order.orderId})!`,
        content: [{
            type: 'text/plain',
            value: `${order.customerName}, your order (${order.orderId}) is being processed!`
        }]
    };
}

export default sendGrid;
