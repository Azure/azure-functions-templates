module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const testRow = {
        "column1": column1Value,
        "column2": "column2Value"
    };

    context.bindings.results = req["body"]?.rows ?? JSON.stringify(testRow);

    return {
        status: 201,
        body: context.bindings.results
    };
}