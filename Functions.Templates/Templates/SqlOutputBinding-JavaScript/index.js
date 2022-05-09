module.exports = async function (context) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const testRow = {
        "column1": column1Value,
        "column2": "column2Value",
        "column3": "column3Value"
    };

    context.bindings.results = JSON.stringify(testRow);

    return {
        status: 201,
        body: context.bindings.results
    };
}