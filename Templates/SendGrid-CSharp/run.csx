#r "SendGridMail"

using System;
using SendGrid;
using Microsoft.Azure.WebJobs.Host;

// To use this template you must configure the app setting "AzureWebJobsSendGridApiKey"
// with your SendGrid Api key. You can also optionally configure the default From/To addresses
// globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
public static void Run(Order order, out SendGridMessage message, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed order: {order.OrderId}");

    message = new SendGridMessage()
    {
        Subject = string.Format("Thanks for your order (#{0})!", order.OrderId),
        Text = string.Format("{0}, your order ({1}) is being processed!", order.CustomerName, order.OrderId)
    };
    message.AddTo(order.CustomerEmail);
}

public class Order
{
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
}