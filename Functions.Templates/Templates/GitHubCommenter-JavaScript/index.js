// Setup
// 1) Go to Function app settings -> App Service settings -> Tools -> Console
//    Enter the following commands: 
//    > cd <functionName>
//    > npm install
// 2) Add the GITHUB_CREDENTIALS as an app setting, Value for the app setting is a base64 encoded string in the following format
//    "Username:Password" or "Username:PersonalAccessToken"
//     Please follow the link https://developer.github.com/v3/oauth/ to get more information on GitHub authentication
module.exports = function (context, payload) {
    if (payload.action != "opened") {
        context.done();
        return;
    }

    var comment = { "body": "Thank you for your contribution, We will get to it shortly" };
    var label = ["bug"];

    if (payload.issue) {
        context.log(payload.issue.user.login, ":posted issue #", payload.issue.number, ":", payload.issue.title);

        //Post a comment 
        SendGitHubRequest(payload.issue.comments_url, comment, context);

        //Add a label
        SendGitHubRequest(payload.issue.url + "/labels", label, context);
    }

    if (payload.pull_request) {
        context.log(payload.pull_request.user.login, ":submitted pull request #", payload.pull_request.number, ":", payload.pull_request.title);

        // posting a comment
        SendGitHubRequest(payload.pull_request.comments_url, comment, context);
    }

    context.done();
};

function SendGitHubRequest(url, requestBody, context) {

    var request = require('request');
    var githubCred = 'Basic ' + process.env.GITHUB_CREDENTIALS;
    request({
        url: url,
        method: 'POST',
        headers: {
            'User-Agent': '<username>',
            'Authorization': githubCred
        },
        json: requestBody
    }, function (error, response, body) {
        if (error) {
            context.log(error);
        } else {
            context.log(response.statusCode, body);
        }
    });
}