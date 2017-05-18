// Please follow the link https://developer.github.com/v3/oauth/ to get information on GitHub authentication

#if (portalTemplates)
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

public static async Task Run(dynamic payload, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Company.Function
{
    public static class GitHubCommenterCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static async Task Run([HttpTrigger(WebHookType = "github")]dynamic payload, TraceWriter log)
#endif
        {
            if (payload.action != "opened")
            {
                return;
            }

            string comment = "{ \"body\": \"Thank you for your contribution, We will get to it shortly\" }";
            string label = "[ \"bug\" ]";

            if (payload.issue != null)
            {
                log.Info($"{payload.issue.user.login} posted an issue #{payload.issue.number}:{payload.issue.title}");

                //Post a comment 
                await SendGitHubRequest(payload.issue.comments_url.ToString(), comment);

                //Add a label
                await SendGitHubRequest($"{payload.issue.url.ToString()}/labels", label);
            }

            if (payload.pull_request != null)
            {
                log.Info($"{payload.pull_request.user.login} submitted pull request #{payload.pull_request.number}:{payload.pull_request.title}");

                // posting a comment
                await SendGitHubRequest(payload.pull_request.comments_url.ToString(), comment);
            }
        }

        public static async Task SendGitHubRequest(string url, string requestBody)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("username", "version"));

                // Add the GITHUB_CREDENTIALS as an app setting, Value for the app setting is a base64 encoded string in the following format
                // "Username:Password" or "Username:PersonalAccessToken"
                // Please follow the link https://developer.github.com/v3/oauth/ to get more information on GitHub authentication 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Environment.GetEnvironmentVariable("GITHUB_CREDENTIALS"));
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                await client.PostAsync(url, content);
            }
        }
#if (vsTemplates)
    }
}
#endif