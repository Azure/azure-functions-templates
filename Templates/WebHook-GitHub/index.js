module.exports = function (context, data) {
    context.res = {};
    context.log('GitHub WebHook triggered! ' + data.comment.body);
    context.res.body = 'New GitHub comment: ' + JSON.stringify(data.comment.body, null, " ");
    context.done();
}