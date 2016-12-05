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
public static Mail Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
    
    Mail message = new Mail()
    {
        Subject = $"Daily Report"
    };

	var orderCount = 100;

    Content content = new Content
    {
        Type = "text/plain",
        Value = $"You had ${orderCount} orders today!"
    };

    message.AddContent(content);    
    return message;
}
