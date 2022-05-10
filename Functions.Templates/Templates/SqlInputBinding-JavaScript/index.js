module.exports = async function (context, results) {
    context.log('Query results: ', JSON.stringify(results));
    return {
        status: 200,
        body: results
    };
}
