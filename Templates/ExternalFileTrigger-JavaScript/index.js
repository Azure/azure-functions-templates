module.exports = function (context, input) {
    context.log('JavaScript External trigger function processed a file!');
    context.done(null, {
        output: input
    });
};