var util = require('util');

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
module.exports = function (context, order) {
    context.log('JavaScript queue trigger function processed order', order.orderId);

    context.done(null, {
        message: {
            subject: util.format('Thanks for your order (#%d)!', order.orderId),
            content: [{
                type: 'text/plain',
                value: util.format("%s, your order (%d) is being processed!", order.customerName, order.orderId)
            }]
        }
    });
}