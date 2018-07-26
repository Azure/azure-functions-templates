#r "Microsoft.Graph"
using Microsoft.Graph;
using System.Net;

public static async Task Run(Message msg, ILogger log)  
{
	log.LogInformation("Microsoft Graph webhook trigger function processed a request.");

    // Testable by sending an email with the Subject "[DEMO] Azure Functions" and some text body
    if (msg.Subject.Contains("[DEMO] Azure Functions")) {
        log.LogInformation($"Processed email: {msg.BodyPreview}");
    }
}