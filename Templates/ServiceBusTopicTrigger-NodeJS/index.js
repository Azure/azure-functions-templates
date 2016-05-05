module.exports = function(context, mySbMsg) {
    context.log('Node.js ServiceBus topic trigger function processed message', mySbMsg);
    context.done();
};