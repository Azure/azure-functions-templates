#r "Microsoft.Graph"
using Microsoft.Graph;
using System.Net;

public static async Task Run(Message msg, TraceWriter log)  
{
	log.Info("Microsoft Graph webhook trigger function processed a request.");

    // Testable by sending oneself an email with the Subject "[DEMO] Azure Functions" and some text body
    if (msg.Subject.Contains("[DEMO] Azure Functions")) {
        log.Info($"Processed email: {msg.BodyPreview}");
    }
}