module.exports = function(context, myQueueItem) {
    context.log('Node.js ServiceBus queue trigger function processed message', myQueueItem);

    var OutPutQueueItem = null;
    if (myQueueItem.count < 1) {
        // write a message back to the queue that this function is triggered on
        // ensuring that we only loop on this once
        myQueueItem.count += 1;
        OutPutQueueItem = {
            message: JSON.stringify(myQueueItem)
        };
    }

    context.done(null, OutPutQueueItem);
};