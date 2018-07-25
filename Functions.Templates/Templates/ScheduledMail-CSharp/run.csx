#r "SendGrid"

using System;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs.Host;

// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
public static SendGridMessage Run(TimerInfo myTimer, ILogger log)
{
    var today = DateTime.Today.ToShortDateString();
    log.LogInformation($"Generating daily report for {today} at {DateTime.Now}");
    
    SendGridMessage message = new SendGridMessage()
    {
        Subject = $"Daily Report for {today}"
    };

    // TODO: Customize this code to generate your specific SendGridMessage message
    var orderCount = 100;

    message.AddContent("text/plain",$"You had {orderCount} orders today!");
    return message;
}
