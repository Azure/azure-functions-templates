#r "Newtonsoft.Json"

using System.Configuration;
using System.Text;

using Newtonsoft.Json.Linq;

// This sample demonstrates a simple use case of getting real-time availability percentage 
// over last 20 minutes onto Power BI at per minute refresh with specified target availability percentage.

// Note that you can also change the query to use different metrics/segments/aggregations/filters for your need

// Also note that about [API Rate limits](https://dev.applicationinsights.io/documentation/Authorization/Rate-limits)
// So it would be wise to disable the function (Your Function > Manage > Disabled) when not in use

// For the Power BI to work, you need to provide the "Push URL" which can be obtained as follows:
// app.powerbi.com > new dashboard > Add tile > Custom Streaming Data > Add streaming dataset > API > Next >
// Add a field with name `ts` and type DateTime (represents timestamp of calculation)
// Add a field with name `availability` and type Number (represents availability percentage for the interval)
// Add a field with name `target` and type Number (represents targeting percentage)
// Add a field with name `min` and type Number (represents minimum percentage)
// Add a field with name `max` and type Number (represents maximum percentage)
// Create > Copy the "Push URL" and paste it as the value of {RealTimePushURL}

// For questions or feedbacks, please visit [Application Insights forum] https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights

private const string AppInsightsApi = "https://api.applicationinsights.io/beta/apps";

// [CONFIGURATION_REQUIRED] configure {RealTimePushURL} accordingly with values obtained from Power BI
// [Get REST API URL endpoint] https://powerbi.microsoft.com/documentation/powerbi-service-real-time-streaming
private const string RealTimePushURL = "REAL_TIME_PUSH_URL";

// [CONFIGURATION_REQUIRED] configure {AI_APP_ID} and {AI_APP_KEY} accordingly in App Settings with values obtained from Application Insights
// [Get your Application ID and API key] https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID
// [Configure Azure Function Application settings] https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
private static readonly string AiAppId = ConfigurationManager.AppSettings["AI_APP_ID"];
private static readonly string AiAppKey = ConfigurationManager.AppSettings["AI_APP_KEY"];

public static async Task Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    log.Info($"Executing real-time Power BI run at: {DateTime.Now}");

    // [CONFIGURATION_REQUIRED] update accordingly for your scenario
    TimeSpan availabilityInterval = TimeSpan.FromMinutes(20);
    double targetAvailability = 80;

    using (var httpClient = new HttpClient())
    {
        // generate request ID to allow issue tracking
        string requestId = Guid.NewGuid().ToString();
        log.Verbose($"[Verbose]: API request ID is {requestId}");
        httpClient.DefaultRequestHeaders.Add("x-api-key", AiAppKey);
        httpClient.DefaultRequestHeaders.Add("x-ms-app", "FunctionTemplate");
        httpClient.DefaultRequestHeaders.Add("x-ms-client-request-id", requestId);
        string metric = "availabilityResults/count";
        string segment = "availabilityResult/success";
        string aggregation = "sum";
        string from = DateTime.UtcNow.Subtract(availabilityInterval).ToString("o");
        string to = DateTime.UtcNow.ToString("o");
        string apiPath = $"{AppInsightsApi}/{AiAppId}/metrics/{metric}?useMDM=true&clientId={requestId}&timespan={from}/{to}&segment={segment}&aggregation={aggregation}";
        using (var httpResponse = await httpClient.GetAsync(apiPath))
        {
            httpResponse.EnsureSuccessStatusCode();
            var resultJson = await httpResponse.Content.ReadAsAsync<JToken>();
            JToken segments = resultJson.SelectToken("value.segments");
            int segmentCount = segments?.Count() ?? 0;
            long[] results = new long[2];
            for (int i = 0; i < segmentCount; i++)
            {
                int segmentValue = segments.SelectToken($"[{i}].{segment}").ToObject<int>();
                results[segmentValue] = segments.SelectToken($"[{i}].{metric}.{aggregation}").ToObject<long>();
            }

            long passed = results[1];
            long failed = results[0];
            long total = passed + failed;
            string availabilityPercentage = passed > 0 ? (100.0d * passed / total).ToString("N2") : "0";
            string postData = $"[{{ \"ts\": \"{to}\", \"availability\": {availabilityPercentage}, \"target\": {targetAvailability}, \"min\": 0, \"max\": 100 }}]";
            log.Verbose($"[Verbose]: Sending data: {postData}");
            using (var response = await httpClient.PostAsync(RealTimePushURL, new ByteArrayContent(Encoding.UTF8.GetBytes(postData))))
            {
                log.Verbose($"[Verbose]: Data sent with response: {response.StatusCode}");
            }
        }
    }
}
