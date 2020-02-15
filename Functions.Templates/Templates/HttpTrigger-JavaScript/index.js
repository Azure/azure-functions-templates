module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.query.name || (req.body && req.body.name)) {
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "Hello " + (req.query.name || req.body.name) + ", The HTTP trigger function executed succefully"
        };
    }
    else {
        context.res = {
            // status: 200, /* Defaults to 200 */
            body: "The HTTP trigger function executed succefully. Pass a name in the query string or in the request body for a personalized response."
        };
    }
};