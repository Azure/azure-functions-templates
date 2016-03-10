module.exports = function (body, context) {
    context.log('GitHub WebHook triggered! ' + body.comment.body);
    context.res = {};
    context.res.body = 'New GitHub comment: ' + JSON.stringify(body.comment.body, null, " ");
    context.done();
}