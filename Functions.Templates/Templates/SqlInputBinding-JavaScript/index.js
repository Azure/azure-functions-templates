module.exports = async function (context, results) {
    context.log('Query results: ', JSON.stringify(results));
    return {
        status: 200,
        mimetype:"application/json",
        body: results
    };
}
