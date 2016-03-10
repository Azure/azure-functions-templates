module.exports = function (body, context) {
    context.log('GitHub WebHook triggered! ' + body.comment.body);
    context.done(null, {
        res: {
            body: 'New GitHub comment: ' + JSON.stringify(body.comment.body, null, " ")
        }
    });
}