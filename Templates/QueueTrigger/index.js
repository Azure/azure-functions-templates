module.exports = function (myQueue, context) {
    context.log('Node.js queue trigger function processed work item ' + myQueue.id);
    context.done();
}