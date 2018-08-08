module.exports = async function(context, mySbMsg) {
    context.log('JavaScript ServiceBus queue trigger function processed message', mySbMsg);
};