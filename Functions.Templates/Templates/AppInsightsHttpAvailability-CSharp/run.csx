using System.Configuration;
using System.Diagnostics;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

// This sample demonstrates a simple use case of calling your web app every 5 minutes.

// Note that you can also create multiple functions from multiple [Azure regions](https://azure.microsoft.com/en-us/regions) 
// to monitor the availability from multiple locations around the world.
// RunLocation will be configured accordingly from these regions.

// For questions or feedbacks, please visit [Application Insights forum] https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights

// setup synthetic headers used for client-server telemetry correlation
private const string SyntheticTestId = "SyntheticTest-Id";
private const string SyntheticTestRunId = "SyntheticTest-RunId";
private const string SyntheticTestLocation = "SyntheticTest-Location";

// [CONFIGURATION_REQUIRED] configure {AI_IKEY} accordingly in App Settings with Instrumentation Key obtained from Application Insights
// [Get an Application Insights Instrumentation Key] https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource
// [Configure Azure Function Application settings] https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
private static readonly TelemetryClient TelemetryClient = new TelemetryClient { InstrumentationKey = ConfigurationManager.AppSettings["AI_IKEY"] };

// [CONFIGURATION_REQUIRED] configure test timeout accordingly for which your request should run
private static readonly HttpClient HttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

public static async Task Run(TimerInfo myTimer, TraceWriter log)
{
    if (myTimer.IsPastDue)
    {
        log.Warning($"[Warning]: Timer is running late! Last ran at: {myTimer.ScheduleStatus.Last}");
    }

    // [CONFIGURATION_REQUIRED] provide {testName} accordingly for your test function
    string testName = "AvailabilityTestFunction";
    if (!HttpClient.DefaultRequestHeaders.Contains(SyntheticTestId))
    {
        HttpClient.DefaultRequestHeaders.Add(SyntheticTestId, testName);
    }

    // REGION_NAME is a default environment variable that comes with App Service
    string location = Environment.GetEnvironmentVariable("REGION_NAME");
    if (!HttpClient.DefaultRequestHeaders.Contains(SyntheticTestLocation))
    {
        HttpClient.DefaultRequestHeaders.Add(SyntheticTestLocation, location);
    }

    // [CONFIGURATION_REQUIRED] configure {uri} and {contentMatch} accordingly for your web app
    await AvailabilityTestRun(
        name: testName,
        location: location,
        uri: "https://azure.microsoft.com/en-us/services/application-insights",
        contentMatch: "Application Insights",
        log: log
    );
}

private static async Task AvailabilityTestRun(string name, string location, string uri, string contentMatch, TraceWriter log)
{
    log.Info($"Executing availability test run for {name} at: {DateTime.Now}");

    // generate operation ID to allow issue tracking
    string operationId = Guid.NewGuid().ToString();
    log.Verbose($"[Verbose]: Operation ID is {operationId}");

    // always update the run Id for every run
    if (HttpClient.DefaultRequestHeaders.Contains(SyntheticTestRunId))
    {
        HttpClient.DefaultRequestHeaders.Remove(SyntheticTestRunId);
    }

    HttpClient.DefaultRequestHeaders.Add(SyntheticTestRunId, operationId);

    var availability = new AvailabilityTelemetry
    {
        Id = operationId,
        Name = name,
        RunLocation = location,
        Success = false
    };
    availability.Context.Operation.Id = operationId;
    availability.Properties.Add("TestUri", uri);
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    bool isMonitoringFailure = false;

    try
    {
        using (var httpResponse = await HttpClient.GetAsync(uri))
        {
            // add test results to availability telemetry property
            availability.Properties.Add("HttpResponseStatusCode", Convert.ToInt32(httpResponse.StatusCode).ToString());

            // check if response content contains specific text
            string content = httpResponse.Content != null ? await httpResponse.Content.ReadAsStringAsync() : "";
            availability.Properties.Add("HttpResponseContent", content);
            if (httpResponse.IsSuccessStatusCode && content.Contains(contentMatch))
            {
                availability.Success = true;
                availability.Message = $"Test succeeded with response: {httpResponse.StatusCode}";
                log.Verbose($"[Verbose]: {availability.Message}");
            }
            else if (!httpResponse.IsSuccessStatusCode)
            {
                availability.Message = $"Test failed with response: {httpResponse.StatusCode}";
                log.Warning($"[Warning]: {availability.Message}");
            }
            else
            {
                availability.Message = $"Test content does not contain: {contentMatch}";
                log.Warning($"[Warning]: {availability.Message}");
            }
        }
    }
    catch (TaskCanceledException e)
    {
        availability.Message = $"Test timed out: {e.Message}";
        log.Warning($"[Warning]: {availability.Message}");
    }
    catch (Exception ex)
    {
        // track exception when unable to determine the state of web app
        isMonitoringFailure = true;
        var exceptionTelemetry = new ExceptionTelemetry(ex);
        exceptionTelemetry.Context.Operation.Id = operationId;
        exceptionTelemetry.Properties.Add("TestName", name);
        exceptionTelemetry.Properties.Add("TestLocation", location);
        exceptionTelemetry.Properties.Add("TestUri", uri);
        TelemetryClient.TrackException(exceptionTelemetry);
        log.Error($"[Error]: {ex.Message}");

        // optional - throw to fail the function
        throw;
    }
    finally
    {
        stopwatch.Stop();
        availability.Duration = stopwatch.Elapsed;
        availability.Timestamp = DateTimeOffset.UtcNow;

        // do not make assumption for the state of web app when is monitoring failure
        if (!isMonitoringFailure)
        {
            TelemetryClient.TrackAvailability(availability);
            log.Info($"Availability telemetry for {name} is sent.");
        }

        // call flush to ensure telemetries are sent
        TelemetryClient.Flush();
    }
}