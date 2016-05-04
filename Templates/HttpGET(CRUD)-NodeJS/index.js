module.exports = function (context, req, intable) {
    context.log("Retrieved records:", intable);
    context.res = {
        status: 200,
        body: intable
    };
    context.done();
};