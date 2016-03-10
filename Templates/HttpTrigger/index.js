module.exports = function (context, req) {
    context.log('Node.js HTTP trigger function processed a request. Name=' + req.originalUrl);

    if (typeof req.query.name == 'undefined') {
        context.res = {
            status: 400,
            body: "Please pass a name on the query string"
        }
    }
    else {
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "Hello " + req.query.name
        }
    }
    context.done();
}
