# AppInsightsScheduledAnalytics - C<span>#</span>

The `AppInsightsScheduledAnalytics` makes it incredibly easy to generate derived metric from your query. 
This sample demonstrates a simple use case of generating derived metric every 5 minutes.

> Note that the idea with this derived metric is that you can easily use it to setup _query-like_ alerting or dashboard. 
> Another important usage of this is to support Autoscale, you can use this derived metric as the source for an Autoscale setting. 
> [Get started with auto scale by custom metric in Azure](https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/monitoring-autoscale-scale-by-custom-metric)

## How it works

For a `AppInsightsScheduledAnalytics` to work, 
you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression) (See the link for full details). 
A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. 
The pattern we use to represent every 5 minutes is `0 */5 * * * *`. 
This, in plain text, means: "When seconds is equal to 0, minutes is divisible by 5, for any hour, day of the month, month, day of the week, or year".

You also have to provide the `Application Insights Instrumentation Key`, 
`Application Insights Application ID` and `Application Insights API Access KEY` with `Read telemetry` access, 
by adding `AI_IKEY`, `AI_APP_ID` and `AI_APP_KEY` repectively in the `Application settings`.

Also please be sure to execute the query in `Application Insights Analytics` portal first for validation of the query!

## Learn more

Here's how you can [Create an Application Insights resource](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)  
Here's how you can [Get your Application ID and API key](https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID)  
Here's how you can [Configure Azure Function Application settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings)  
Here's how you can [Setup alerts](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-alerts)  
Here's where you can find out more about [Application Insights Analytics](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-analytics)  
Here's where you can find out more about [Application Insights Dashboards](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-dashboards)

## Feedbacks or Questions

Please visit [Application Insights forum](https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights)
