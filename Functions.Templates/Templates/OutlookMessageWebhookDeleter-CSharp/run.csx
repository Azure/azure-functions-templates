#r "Microsoft.Graph"
using Microsoft.Graph;
using System.Net;

public static async Task Run(Message msg, string[] existingSubscriptions, ICollector<string> subscriptionsToDelete, TraceWriter log)  
{
    log.Info("C# HTTP trigger function processed a request.");
	foreach (var subscription in existingSubscriptions)
	{
		log.Info($"Deleting subscription {subscription}");
        subscriptionsToDelete.Add(subscription);
    }
}