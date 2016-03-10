module.exports = function (context, body) {
    context.log('Webhook was triggered!');
    context.res = {};
    context.res.body = 'WebHook processed successfully! \n' + JSON.stringify(body, null, " ");
    context.done();
}