module.exports = function (context, req) {
    var statusCode = 400;
    var responseBody = "Invalid request object";

    if (typeof req.body != 'undefined' && typeof req.body == 'object') {
        statusCode = 201;
        context.bindings.outTable = req.body;
        responseBody = "Table Storage Created";
    }

    context.res = {
        status: statusCode,
        body: responseBody
    };

    context.done();
};