module.exports = function (workItem, context) {
    context.log('Node.js eventhub trigger function processed work item ' + workItem);

    context.done(null, workItem);
}