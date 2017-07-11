#r "Microsoft.Graph"

using System;
using System.Net;
using Microsoft.Graph;

public static void Run(TimerInfo timer, TraceWriter log,
    EmailRow[] customers, BodyRow[] body,
    out EmailRow[] update, ICollector<Message> emails)
{
    foreach (var row in customers)
    {
        var email = new Message
        {
            Subject = body[0].Subject,
            Body = new ItemBody
            {
                Content = body[0].Body,
                ContentType = BodyType.Html
            },
            ToRecipients = new Recipient[] {
                new Recipient {
                    EmailAddress = new EmailAddress {
                        Address = row.Email,
                        Name = row.Name
                    }
                }
            }
        };
        emails.Add(email);
        row.Sent = DateTime.Now.ToString();
    }
    log.Info($"Sending emails to {customers.Length} customers; updating table.");
    update = new EmailRow[customers.Length];
    Array.Copy(customers, update, customers.Length);
}

public class EmailRow
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Sent { get; set; }
}

public class BodyRow
{
    public string Body { get; set; }
    public string Subject { get; set; }
}