module.exports = function (context, input) {
    context.log('Node.js manually triggered function called with input ' + JSON.stringify(input));
    context.done();
};