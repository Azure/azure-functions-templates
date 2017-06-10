# AppInsightsRealtimePowerBI - C<span>#</span>

The `AppInsightsRealtimePowerBI` makes it incredibly easy to push real-time data from Application Insights to Power BI. 
This sample demonstrates a simple use case of getting real-time availability percentage 
over last 20 minutes on Power BI at per minute refresh with specified target availability percentage.

> Note that you can also change the query to use different metrics/segments/aggregations/filters for your need  

> Also note that about [API Rate limits](https://dev.applicationinsights.io/documentation/Authorization/Rate-limits)  
> So it would be wise to disable the function (Your Function > Manage > Disabled) when not in use

## How it works

For a `AppInsightsRealtimePowerBI` to work, 
you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression) (See the link for full details). 
A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. 
The pattern we use to represent every day is `0 * * * * *`. 
This, in plain text, means: "When seconds is equal to 0, for any minute, hour, day of the month, month, day of the week, or year".

You also have to provide the `Application Insights Application ID` and `Application Insights API Access KEY` with `Read telemetry` access, 
by adding `AI_APP_ID` and `AI_APP_KEY` in the `Application settings`.

For the Power BI to work, you need to provide the **Push URL** which can be obtained as follows:  
app.powerbi.com > new dashboard > Add tile > Custom Streaming Data > Add streaming dataset > API > Next >  
Add a field with name `ts` and type DateTime (represents timestamp of calculation)  
Add a field with name `availability` and type Number (represents availability percentage for the interval)  
Add a field with name `target` and type Number (represents targeting percentage)  
Add a field with name `min` and type Number (represents minimum percentage)  
Add a field with name `max` and type Number (represents maximum percentage)  
Create > Copy the **"Push URL"** and paste it as the value of {RealTimePushURL}  

## Learn more

Here's how you can [Create an Application Insights resource](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)  
Here's how you can [Get your Application ID and API key](https://dev.applicationinsights.io/documentation/Authorization/API-key-and-App-ID)  
Here's how you can [Configure Azure Function Application settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings)  
Here's where you can find out more about [Power BI REST API for real-time data push](https://msdn.microsoft.com/en-us/library/dn877544.aspx)  
Here's where you can find out more about [Application Insights data access REST API](https://dev.applicationinsights.io/documentation/Using-the-API/Power-BI)  

## Feedbacks or Questions

Please visit [Application Insights forum](https://social.msdn.microsoft.com/Forums/vstudio/en-US/home?forum=ApplicationInsights)
