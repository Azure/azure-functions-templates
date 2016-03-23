﻿module.exports = function (message, context) {
    context.log('Node.js ServiceBus queue trigger function processed message', message);

    var output = null;
    if (message.count < 1)
    {
        // write a message back to the queue that this function is triggered on
        // ensuring that we only loop on this once
        message.count += 1;
        output = {
            message: JSON.stringify(message)
        };
    }

    context.done(null, output);
}