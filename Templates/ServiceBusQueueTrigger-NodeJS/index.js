module.exports = function(context, sbQueueItem) {
    context.log('Node.js ServiceBus queue trigger function processed message', sbQueueItem);
    context.done();
};