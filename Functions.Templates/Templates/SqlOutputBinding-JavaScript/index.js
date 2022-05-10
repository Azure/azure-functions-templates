module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    var rows = req["body"]?.rows ?? [];
    context.bindings.results = JSON.stringify(rows);
	
    return {
        // status: 200, /* Defaults to 200 */
        body: context.bindings.results
    };
}