module.exports = function (context, body) {
    context.log('Webhook was triggered!');
    context.done(null, { 
        res: { 
            body: 'WebHook processed successfully! \n' + JSON.stringify(body, null, " ") 
        } 
    });
}