using System.Configuration;
using System.Diagnostics;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

// [CONFIGURATION_REQUIRED] configure {AI_IKEY} accordingly in App Settings with Instrumentation Key obtained from Application Insights
// [Get an Application Insights Instrumentation Key] https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource
// [Configure Azure Function Application settings] https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
private static readonly TelemetryClient TelemetryClient = new TelemetryClient { InstrumentationKey = ConfigurationManager.AppSettings["AI_IKEY"] };
private static readonly HttpClient HttpClient = new HttpClient();

public static async Task Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    // [CONFIGURATION_REQUIRED] configure {uri} accordingly for your web app
    await AvailabilityTestRun(
        name: "AvailabilityTestFunction",
        uri: "https://azure.microsoft.com/en-us/services/application-insights",
        log: log
    );
}

private static async Task AvailabilityTestRun(string name, string uri, TraceWriter log)
{
    log.Info($"Executing availability test run for {name} at: {DateTime.Now}");

    try
    {
        // REGION_NAME is a default environment variable that comes with App Service
        AvailabilityTelemetry availability = new AvailabilityTelemetry
        {
            Name = name,
            RunLocation = Environment.GetEnvironmentVariable("REGION_NAME")
        };
        availability.Properties.Add("TestUri", uri);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        using (var httpResponse = await HttpClient.GetAsync(uri))
        {
            // add test results to availability telemetry property
            availability.Success = httpResponse.IsSuccessStatusCode;
            availability.Properties.Add("HttpResponseStatusCode", Convert.ToInt32(httpResponse.StatusCode).ToString());
            log.Verbose($"[Verbose]: Test result is {availability.Success} with {httpResponse.StatusCode}");

            // add additional information when test failed
            if (!availability.Success && httpResponse.Content != null)
            {
                availability.Message = await httpResponse.Content.ReadAsStringAsync();
                log.Warning($"[Warning]: {availability.Message}");
            }
        }

        stopwatch.Stop();
        availability.Duration = stopwatch.Elapsed;
        availability.Timestamp = DateTimeOffset.UtcNow;
        TelemetryClient.TrackAvailability(availability);
        log.Info($"Availability telemetry for {name} is sent.");
    }
    catch (Exception ex)
    {
        // track exception when unable to determine the Uri state
        TelemetryClient.TrackException(ex, new Dictionary<string, string>
            {
                {"TestName", name},
                {"TestUri", uri}
            });
        log.Error($"[Error]: {ex.Message}");

        // optional - throw to fail the function
        throw;
    }
    finally
    {
        // call flush to ensure telemetries are sent
        TelemetryClient.Flush();
    }
}
