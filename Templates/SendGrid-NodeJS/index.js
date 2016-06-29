var util = require('util');

// To use this template you must configure the app setting "AzureWebJobsSendGridApiKey"
// with your SendGrid Api key. You can also optionally configure the default From/To addresses
// globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
module.exports = function (context, order) {
    context.log('Node.js queue trigger function processed order', order.orderId);

    context.done(null, {
        message: {
            subject: util.format('Thanks for your order (#%d)!', order.orderId),
            text: util.format("%s, your order (%d) is being processed!", order.customerName, order.orderId)
        }
    });
}