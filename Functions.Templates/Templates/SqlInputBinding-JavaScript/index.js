module.exports = async function (context, results) {
    if (!!results && results.length > 0) {
        context.log('Query results: ', JSON.stringify(results));
    }
}
