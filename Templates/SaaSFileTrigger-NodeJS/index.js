module.exports = function (context, input) {
    context.log('Node.js SaaS trigger function processed a file!');
    context.done(null, {
        output: input
    });
};