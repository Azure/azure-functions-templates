#r "Newtonsoft.Json"

using System.Configuration;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json.Linq;

// This sample demonstrates a simple use case of generating derived metric every 5 minutes.

// Note that the idea with this derived metric is that you can easily use it to setup _query-like_ alerting or dashboard.
// Another important usage of this is to support Autoscale, you can use this derived metric as the source for an Autoscale setting.
// [Get started with auto scale by custom metric in Azure] https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-autoscale-scale-by-custom-metric

// For questions or feedbacks, please visit [Application Insights forum] https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights

private const string AppInsightsApi = "https://api.applicationinsights.io/beta/apps";

// [CONFIGURATION_REQUIRED] configure {AI_IKEY} accordingly in App Settings with Instrumentation Key obtained from Application Insights
// [Get an Application Insights Instrumentation Key] https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource
// [Configure Azure Function Application settings] https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
private static readonly TelemetryClient TelemetryClient = new TelemetryClient { InstrumentationKey = ConfigurationManager.AppSettings["AI_IKEY"] };

// [CONFIGURATION_REQUIRED] configure {AI_APP_ID} and {AI_APP_KEY} accordingly in App Settings with values obtained from Application Insights
// [Get your Application ID and API key] https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID
private static readonly string AiAppId = ConfigurationManager.AppSettings["AI_APP_ID"];
private static readonly string AiAppKey = ConfigurationManager.AppSettings["AI_APP_KEY"];

public static async Task Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    // [CONFIGURATION_REQUIRED] update the query accordingly for your need
    // be sure to run it against Application Insights Analytics portal first for validation
    // output should be a number if sending derived metrics
    // [Application Insights Analytics] https://docs.microsoft.com/en-us/azure/application-insights/app-insights-analytics
    await ScheduledAnalyticsRun(
        name: "ScheduledAnalyticsFunction",
        query: @"
requests 
| where timestamp > ago(1h) 
| summarize passed = countif(success == true), total = count() 
| project passed * 1.0 / total * 100
",
        log: log
    );
}

private static async Task ScheduledAnalyticsRun(string name, string query, TraceWriter log)
{
    log.Info($"Executing scheduled analytics run for {name} at: {DateTime.Now}");

    // generate request ID to allow issue tracking
    string requestId = Guid.NewGuid().ToString();
    log.Verbose($"[Verbose]: API request ID is {requestId}");

    try
    {
        MetricTelemetry metric = new MetricTelemetry { Name = name };
        metric.Context.Operation.Id = requestId;
        metric.Properties.Add("TestAppId", AiAppId);
        metric.Properties.Add("TestQuery", query);
        metric.Properties.Add("TestRequestId", requestId);
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("x-api-key", AiAppKey);
            httpClient.DefaultRequestHeaders.Add("x-ms-app", "FunctionTemplate");
            httpClient.DefaultRequestHeaders.Add("x-ms-client-request-id", requestId);
            string apiPath = $"{AppInsightsApi}/{AiAppId}/query?clientId={requestId}&timespan=P1D&query={query}";
            using (var httpResponse = await httpClient.GetAsync(apiPath))
            {
                // throw exception when unable to determine the metric value
                httpResponse.EnsureSuccessStatusCode();
                var resultJson = await httpResponse.Content.ReadAsAsync<JToken>();
                double result;
                if (double.TryParse(resultJson.SelectToken("Tables[0].Rows[0][0]")?.ToString(), out result))
                {
                    metric.Sum = result;
                    log.Verbose($"[Verbose]: Metric result is {metric.Sum}");
                }
                else
                {
                    log.Error($"[Error]: {resultJson.ToString()}");
                    throw new FormatException("Query must result in a single metric number. Try it on Analytics before scheduling.");
                }
            }
        }

        TelemetryClient.TrackMetric(metric);
        log.Info($"Metric telemetry for {name} is sent.");
    }
    catch (Exception ex)
    {
        // track exception when unable to determine the metric value
        var exceptionTelemetry = new ExceptionTelemetry(ex);
        exceptionTelemetry.Context.Operation.Id = requestId;
        exceptionTelemetry.Properties.Add("TestName", name);
        exceptionTelemetry.Properties.Add("TestAppId", AiAppId);
        exceptionTelemetry.Properties.Add("TestQuery", query);
        exceptionTelemetry.Properties.Add("TestRequestId", requestId);
        TelemetryClient.TrackException(exceptionTelemetry);
        log.Error($"[Error]: Client Request ID {requestId}: {ex.Message}");

        // optional - throw to fail the function
        throw;
    }
    finally
    {
        TelemetryClient.Flush();
    }
}
