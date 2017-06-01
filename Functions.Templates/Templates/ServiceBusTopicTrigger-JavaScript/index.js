module.exports = function(context, mySbMsg) {
    context.log('JavaScript ServiceBus topic trigger function processed message', mySbMsg);
    context.done();
};