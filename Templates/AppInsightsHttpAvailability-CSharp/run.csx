using System.Configuration;
using System.Diagnostics;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

private static TelemetryClient telemetryClient = new TelemetryClient { InstrumentationKey = ConfigurationManager.AppSettings["AIIKEY"] };
private static HttpClient httpClient = new HttpClient();

public static async Task Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    log.Info($"Executing availability test run at: {DateTime.Now}");
    await AvailabilityTestRun(
        name: "AvailabilityTestFunction",
        uri: "https://azure.microsoft.com/en-us/services/application-insights",
        log: log
    );
}

private static async Task AvailabilityTestRun(string name, string uri, TraceWriter log)
{
    log.Info($"Executing availability test run for {name}.");

    try
    {
        AvailabilityTelemetry availability = new AvailabilityTelemetry
        {
            Name = name,
            RunLocation = Environment.GetEnvironmentVariable("REGION_NAME")
        };
        availability.Properties.Add("TestUri", uri);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        using (var httpResponse = await httpClient.GetAsync(uri))
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
        telemetryClient.TrackAvailability(availability);
        log.Info($"Availability telemetry for {name} is sent.");
    }
    catch (Exception ex)
    {
        // track exception when unable to determine the Uri state
        telemetryClient.TrackException(ex, new Dictionary<string, string>
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
        telemetryClient.Flush();
    }
}