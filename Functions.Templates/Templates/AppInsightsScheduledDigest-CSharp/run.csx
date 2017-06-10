#r "Newtonsoft.Json"
#r "SendGrid"

using System.Configuration;

using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;

// This sample demonstrates a simple use case of getting digest email once a day.

// Note that you can also update the query for weekly or monthly digest reports, or even to provide deeper insights.

// For questions or feedbacks, please visit [Application Insights forum] https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights

private const string AppInsightsApi = "https://api.applicationinsights.io/beta/apps";

// [CONFIGURATION_REQUIRED] configure {AI_APP_ID} and {AI_APP_KEY} accordingly in App Settings with values obtained from Application Insights
// [Get your Application ID and API key] https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID
// [Configure Azure Function Application settings] https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
private static readonly string AiAppId = ConfigurationManager.AppSettings["AI_APP_ID"];
private static readonly string AiAppKey = ConfigurationManager.AppSettings["AI_APP_KEY"];

// [CONFIGURATION_REQUIRED] configure {SendGridApiKey} accordingly in App Settings with API Key obtained from SendGrid
// [Obtain SendGrid API Key] https://sendgrid.com/docs/Classroom/Basics/API/what_is_my_api_key.html
public static async Task<Mail> Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    DigestResult result = await ScheduledDigestRun(
        query: GetQueryString(),
        log: log
    );

    // [CONFIGURATION_REQUIRED] configure {appName} accordingly for your app/email
    string appName = "Your";
    var today = DateTime.Today.ToShortDateString();
    Content content = new Content
    {
        Type = "text/html",
        Value = GetHtmlContentValue(appName, today, result)
    };
    Mail message = new Mail()
    {
        Subject = $"Your daily Application Insights digest report for {today}"
    };
    message.AddContent(content);

    log.Info($"Generating daily report for {today} at {DateTime.Now}");
    return message;
}

private static async Task<DigestResult> ScheduledDigestRun(string query, TraceWriter log)
{
    log.Info($"Executing scheduled daily digest run at: {DateTime.Now}");

    // generate request ID to allow issue tracking
    string requestId = Guid.NewGuid().ToString();
    log.Verbose($"[Verbose]: API request ID is {requestId}");

    try
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("x-api-key", AiAppKey);
            httpClient.DefaultRequestHeaders.Add("x-ms-app", "FunctionTemplate");
            httpClient.DefaultRequestHeaders.Add("x-ms-client-request-id", requestId);
            string apiPath = $"{AppInsightsApi}/{AiAppId}/query?clientId={requestId}&timespan=P1W&query={query}";
            using (var httpResponse = await httpClient.GetAsync(apiPath))
            {
                // throw exception when unable to determine the metric value
                httpResponse.EnsureSuccessStatusCode();
                var resultJson = await httpResponse.Content.ReadAsAsync<JToken>();
                DigestResult result = new DigestResult
                {
                    TotalRequests = resultJson.SelectToken("Tables[0].Rows[0][0]")?.ToObject<long>().ToString("N0"),
                    FailedRequests = resultJson.SelectToken("Tables[0].Rows[0][1]")?.ToObject<long>().ToString("N0"),
                    RequestsDuration = resultJson.SelectToken("Tables[0].Rows[0][2]")?.ToString(),
                    TotalDependencies = resultJson.SelectToken("Tables[0].Rows[0][3]")?.ToObject<long>().ToString("N0"),
                    FailedDependencies = resultJson.SelectToken("Tables[0].Rows[0][4]")?.ToObject<long>().ToString("N0"),
                    DependenciesDuration = resultJson.SelectToken("Tables[0].Rows[0][5]")?.ToString(),
                    TotalViews = resultJson.SelectToken("Tables[0].Rows[0][6]")?.ToObject<long>().ToString("N0"),
                    TotalExceptions = resultJson.SelectToken("Tables[0].Rows[0][7]")?.ToObject<long>().ToString("N0"),
                    OverallAvailability = resultJson.SelectToken("Tables[0].Rows[0][8]")?.ToString(),
                    AvailabilityDuration = resultJson.SelectToken("Tables[0].Rows[0][9]")?.ToString()
                };
                return result;
            }
        }
    }
    catch (Exception ex)
    {
        log.Error($"[Error]: Client Request ID {requestId}: {ex.Message}");

        // optional - throw to fail the function
        throw;
    }
}

private static string GetQueryString()
{
    // update the query accordingly for your need (be sure to run it against Application Insights Analytics portal first for validation)
    // [Application Insights Analytics] https://docs.microsoft.com/en-us/azure/application-insights/app-insights-analytics
    return @"
requests
| where timestamp > ago(1d)
| summarize Row = 1, TotalRequests = sum(itemCount), FailedRequests = sum(toint(success == 'False')),
    RequestsDuration = iff(isnan(avg(duration)), '------', tostring(toint(avg(duration) * 100) / 100.0))
| join (
dependencies
| where timestamp > ago(1d)
| summarize Row = 1, TotalDependencies = sum(itemCount), FailedDependencies = sum(success == 'False'),
    DependenciesDuration = iff(isnan(avg(duration)), '------', tostring(toint(avg(duration) * 100) / 100.0))
) on Row | join (
pageViews
| where timestamp > ago(1d)
| summarize Row = 1, TotalViews = sum(itemCount)
) on Row | join (
exceptions
| where timestamp > ago(1d)
| summarize Row = 1, TotalExceptions = sum(itemCount)
) on Row | join (
availabilityResults
| where timestamp > ago(1d)
| summarize Row = 1, OverallAvailability = iff(isnan(avg(toint(success))), '------', tostring(toint(avg(toint(success)) * 10000) / 100.0)),
    AvailabilityDuration = iff(isnan(avg(duration)), '------', tostring(toint(avg(duration) * 100) / 100.0))
) on Row
| project TotalRequests, FailedRequests, RequestsDuration, TotalDependencies, FailedDependencies, DependenciesDuration, TotalViews, TotalExceptions, OverallAvailability, AvailabilityDuration
";
}

private static string GetHtmlContentValue(string appName, string today, DigestResult result)
{
    // update the HTML template accordingly for your need
    return $@"
<html><body>
<p style='text-align: center;'><strong>{appName} daily telemetry report {today}</strong></p>
<p style='text-align: center;'>The following data shows insights based on telemetry from last 24 hours.</p>
<table align='center' style='width: 95%; max-width: 480px;'><tbody>
<tr>
<td style='min-width: 150px; text-align: left;'><strong>Total requests</strong></td>
<td style='min-width: 100px; text-align: right;'><strong>{result.TotalRequests}</strong></td>
</tr>
<tr>
<td style='min-width: 120px; padding-left: 5%; text-align: left;'>Failed requests</td>
<td style='min-width: 100px; text-align: right;'>{result.FailedRequests}</td>
</tr>
<tr>
<td style='min-width: 120px; padding-left: 5%; text-align: left;'>Average response time</td>
<td style='min-width: 100px; text-align: right;'>{result.RequestsDuration} ms</td>
</tr>
<tr>
<td colspan='2'><hr /></td>
</tr>
<tr>
<td style='min-width: 150px; text-align: left;'><strong>Total dependencies</strong></td>
<td style='min-width: 100px; text-align: right;'><strong>{result.TotalDependencies}</strong></td>
</tr>
<tr>
<td style='min-width: 120px; padding-left: 5%; text-align: left;'>Failed dependencies</td>
<td style='min-width: 100px; text-align: right;'>{result.FailedDependencies}</td>
</tr>
<tr>
<td style='min-width: 120px; padding-left: 5%; text-align: left;'>Average response time</td>
<td style='min-width: 100px; text-align: right;'>{result.DependenciesDuration} ms</td>
</tr>
<tr>
<td colspan='2'><hr /></td>
</tr>
<tr>
<td style='min-width: 150px; text-align: left;'><strong>Total views</strong></td>
<td style='min-width: 100px; text-align: right;'><strong>{result.TotalViews}</strong></td>
</tr>
<tr>
<td style='min-width: 150px; text-align: left;'><strong>Total exceptions</strong></td>
<td style='min-width: 100px; text-align: right;'><strong>{result.TotalExceptions}</strong></td>
</tr>
<tr>
<td colspan='2'><hr /></td>
</tr>
<tr>
<td style='min-width: 150px; text-align: left;'><strong>Overall Availability</strong></td>
<td style='min-width: 100px; text-align: right;'><strong>{result.OverallAvailability} %</strong></td>
</tr>
<tr>
<td style='min-width: 120px; padding-left: 5%; text-align: left;'>Average response time</td>
<td style='min-width: 100px; text-align: right;'>{result.AvailabilityDuration} ms</td>
</tr>
</tbody></table>
</body></html>
";
}

private struct DigestResult
{
    public string TotalRequests;
    public string FailedRequests;
    public string RequestsDuration;
    public string TotalDependencies;
    public string FailedDependencies;
    public string DependenciesDuration;
    public string TotalViews;
    public string TotalExceptions;
    public string OverallAvailability;
    public string AvailabilityDuration;
}
