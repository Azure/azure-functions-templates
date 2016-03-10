module.exports = function (context, data) {
    context.res = {};
    context.log('Webhook was triggered!');
    context.res.body = 'WebHook processed successfully! \n' + JSON.stringify(data, null, " ");
    context.done();
}