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
#if (portalTemplates)
#r "SendGrid"

using System;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs.Host;

public static Mail Run(Order order, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;

namespace Company.Function
{
    public static class SendGridCSharp
    {
        [FunctionName("SendGridCSharp")]
        [return: SendGrid(ApiKey = "ApiKeyValue",To = "{CustomerEmail}", From = "FromEmailValue" )]
        public static Mail Run([QueueTrigger("PathValue", Connection = "ConnectionValue")]Order order, TraceWriter log)
#endif
    {
        log.Info($"C# Queue trigger function processed order: {order.OrderId}");
        
        Mail message = new Mail()
        {
            Subject = $"Thanks for your order (#{order.OrderId})!"
        };

        Content content = new Content
        {
            Type = "text/plain",
            Value = $"{order.CustomerName}, your order ({order.OrderId}) is being processed!"
        };

        message.AddContent(content);    
        return message;
    }
#if (vsTemplates)
    }
#endif
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
#if (vsTemplates)
}
#endif