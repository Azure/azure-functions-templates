// Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
module.exports = async function (context, req) {
    context.log('HTTP trigger with SQL output binding function processed a request.');

    // Set array to context.bindings for insertion
    context.bindings.results = JSON.stringify(req.body);

    context.res = {
        status: 201,
        body: req.body
    };
}