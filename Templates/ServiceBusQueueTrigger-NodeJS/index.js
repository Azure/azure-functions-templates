module.exports = function(context, mySbQueueMsg) {
    context.log('Node.js ServiceBus queue trigger function processed message', mySbQueueMsg);
    context.done();
};