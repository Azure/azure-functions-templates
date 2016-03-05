module.exports = function (workItem, context) {
    context.log('Node.js blob trigger function processed work item ' + workItem.id);
    context.done();
}