module.exports = function (context, myBlob) {
    context.log('Node.js blob trigger function processed blob:', myBlob);
    context.done();
};