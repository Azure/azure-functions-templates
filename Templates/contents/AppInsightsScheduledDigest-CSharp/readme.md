# AppInsightsScheduledDigest - C<span>#</span>

The `AppInsightsScheduledDigest` makes it incredibly easy to customize your digest emails and execute it on configurable schedule. 
This sample demonstrates a simple use case of getting digest email once a day.

> Note that you can also update the query for weekly or monthly digest reports, or even to provide deeper insights.

## How it works

For a `AppInsightsScheduledDigest` to work, 
you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression) (See the link for full details). 
A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. 
The pattern we use to represent every day is `0 0 2 * * *`. 
This, in plain text, means: "When seconds is equal to 0, minutes is equal to 0, hours is equal to 2, for any day of the month, month, day of the week, or year".

You also have to provide the `Application Insights Application ID` and `Application Insights API Access KEY` with `Read telemetry` access, 
by adding `AI_APP_ID` and `AI_APP_KEY` in the `Application settings`.

Also note that if you are updating the query, be sure to run it through `Application Insights Analytics` portal first for validation of the query!

For the email to work, you need to provide the `SendGridApiKey` in the `Application settings`. 
The one in the `function.json` points to the `Key Name` in the `Application Settings`, 
so create a `Key` named `SendGridApiKey` with the value for the API Key obtained from `SendGrid`.

## Learn more

Here's how you can [Create an Application Insights resource](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)  
Here's how you can [Get your Application ID and API key](https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID)  
Here's how you can [Configure Azure Function Application settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings)  
Here's where you can find out more about [SendGrid API Key](https://sendgrid.com/docs/Classroom/Basics/API/what_is_my_api_key.html)

## Feedbacks or Questions

Please visit [Application Insights forum](https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights)
