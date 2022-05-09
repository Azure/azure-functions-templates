module.exports = async function (context, results) {
    if (!!results && results.length > 0) {
        context.log('Query results: ', JSON.stringify(results));
    } else {
        context.log("No results!");
    }
    return {
        status: 201,
        mimetype:"application/json",
        body: results
    };
}
