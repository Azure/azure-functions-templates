module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    var rows = req["body"]?.rows ?? [];
    if (rows.length > 0) {
        context.bindings.results = JSON.stringify(rows);
    }
    return {
        status: 201,
        mimetype: "application/json",
        body: context.bindings.results
    };
}