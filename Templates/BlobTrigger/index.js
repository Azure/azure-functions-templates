module.exports = function (myBlobTrigger, context) {
    context.log('Node.js blob trigger function processed work item:' + myBlobTrigger.id);
    context.done();
}