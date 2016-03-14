module.exports = function (context, myQueueItem) {
    context.log('Node.js queue trigger function processed work item ' + myQueueItem.id);
    context.done();
};