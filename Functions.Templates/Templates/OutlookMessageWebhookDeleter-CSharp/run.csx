#r "Newtonsoft.Json"
#r "Microsoft.Graph"

using Microsoft.Graph;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task Run(HttpRequest req, Subscription[] existingSubscriptions, IAsyncCollector<string> subscriptionsToDelete, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");
    foreach (var subscription in existingSubscriptions)
        {
            log.LogInformation($"Deleting subscription {subscription.Id}");
            await subscriptionsToDelete.AddAsync(subscription.Id);
        }
}