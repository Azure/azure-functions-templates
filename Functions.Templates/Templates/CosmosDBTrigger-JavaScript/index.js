module.exports = function (context, input) {
    if (!!input && input.length > 0) {
        context.log('Document Id: ', input[0].id);
    }

    context.done();
}
