module.exports = async function (context, myQueueItem) {
    context.log('JavaScript rabbitmq trigger function processed work item', myQueueItem);
};