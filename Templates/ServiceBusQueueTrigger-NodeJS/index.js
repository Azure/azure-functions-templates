module.exports = function(context, mySbMsg) {
    context.log('Node.js ServiceBus queue trigger function processed message', mySbMsg);
    context.done();
};