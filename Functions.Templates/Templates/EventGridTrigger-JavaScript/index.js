module.exports = async function (context, eventGridEvent) {
    context.log(typeof eventGridEvent);
    context.log(eventGridEvent);
};