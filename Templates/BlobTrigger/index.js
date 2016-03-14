module.exports = function (context, myBlob) {
    context.log('Node.js blob trigger function processed blob: ' + myBlob.id);
    context.done();
}