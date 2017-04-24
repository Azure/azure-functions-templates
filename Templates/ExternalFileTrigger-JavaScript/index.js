module.exports = function (context, inputFile) {
    context.log('JavaScript External trigger function processed a file!');
    context.done(null, {
        outputFile: inputFile
    });
};
