# AppInsightsHttpAvailability - C<span>#</span>

The `AppInsightsHttpAvailability` makes it incredibly easy to have a custom function, 
which monitor availability and responsiveness of any web app with Application Insights executed on a schedule. 
This sample demonstrates a simple use case of calling your web app every 5 minutes.

> Note that you can also create multiple `AppInsightsHttpAvailability` function from multiple [Azure regions](https://azure.microsoft.com/en-us/regions) 
> to monitor the availability from multiple locations around the world.  
> RunLocation will be configured accordingly from these regions.

## How it works

For a `AppInsightsHttpAvailability` to work, 
you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression) (See the link for full details). 
A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. 
The pattern we use to represent every 5 minutes is `0 */5 * * * *`. 
This, in plain text, means: "When seconds is equal to 0, minutes is divisible by 5, for any hour, day of the month, month, day of the week, or year".

You also have to provide the `Application Insights Instrumentation Key` by adding `AI_IKEY` in the `Application settings`.

## Learn more

Here's how you can [Create an Application Insights resource](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)  
Here's how you can [Configure Azure Function Application settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings)

## Feedbacks or Questions

Please visit [Application Insights forum](https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights)
