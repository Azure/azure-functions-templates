module.exports = function (context, myBlob) {
    context.log('Node.js blob trigger function processed work item:' + myBlob.id);
    context.done();
};