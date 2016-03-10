module.exports = function (myQueueItem, context) {
    context.log('Node.js queue trigger function processed work item ' + myQueueItem.id);
    context.done();
}