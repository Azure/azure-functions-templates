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
public static SendGridMessage Run(Order order, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed order: {order.OrderId}");
    
    SendGridMessage message = new SendGridMessage()
    {
        Subject = $"Thanks for your order (#{order.OrderId})!"
    };    

    message.AddContent("text/plain",$"{order.CustomerName}, your order ({order.OrderId}) is being processed!");    
    return message;
}

public class Order
{
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
}