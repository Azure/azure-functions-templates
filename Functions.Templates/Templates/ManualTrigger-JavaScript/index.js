module.exports = function (context, input) {
    context.log('JavaScript manually triggered function called with input:', input);
    context.done();
};