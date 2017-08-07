#r "Microsoft.Graph"

using System;
using System.Net;
using Microsoft.Graph;

/// <summary>
/// 1. Read Excel table
/// 2. Use that information to send an email to each customer
/// 3. Update Excel table with the time an email was last sent.
/// </summary>
public static void Run(TimerInfo timer, TraceWriter log,
    EmailRow[] customers, out EmailRow[] update, ICollector<Message> emails)
{
    foreach (var row in customers)
    {
        var email = new Message
        {
            Subject = "Greetings from Azure Functions",
            Body = new ItemBody
            {
                Content = "<h1> hi! </h1>",
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

/// <summary>
/// POCO type; fields match table header names exactly
/// </summary>
public class EmailRow
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Sent { get; set; }
}