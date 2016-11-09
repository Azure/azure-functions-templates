module.exports = function (context, input) {
    context.log('JavaScript SaaS trigger function processed a file!');
    context.done(null, {
        output: input
    });
};