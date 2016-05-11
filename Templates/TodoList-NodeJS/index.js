module.exports = function (context, req, inputList) {
    var method = req.method.toLowerCase();
    methods[method](context, req, inputList);
    context.done();
};

var methods = {
    get: function (context, req, inputList) {
        var itemList = [];

        // Filtering the input list to only get Item and completed column
        for (var i in inputList) {
            itemList.push({
                "item": inputList[i].item, "completed": inputList[i].completed
            });
        }

        context.res = {
            body: itemList
        };
    },
    post: function (context, req) {
        var statusCode = 400;
        var responseBody = "Invalid request object";

        if (typeof req.body != 'undefined' && typeof req.body == 'object') {
            statusCode = 201;
            context.bindings.outList = req.body;
            responseBody = "List Item Created";
        }

        context.res = {
            status: statusCode,
            body: responseBody
        };
    }
};