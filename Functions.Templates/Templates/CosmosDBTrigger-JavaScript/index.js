module.exports = function (context, input) {
    context.log('Document Id: ', input[0].length);

    context.done();
}