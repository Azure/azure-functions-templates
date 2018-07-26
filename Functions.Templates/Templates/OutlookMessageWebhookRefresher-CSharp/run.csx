#r "Microsoft.Azure.WebJobs.Extensions.Tokens"
#r "Microsoft.Azure.WebJobs.Extensions.O365"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Azure.WebJobs;

public static async Task Run(TimerInfo myTimer, UserSubscription[] existingSubscriptions, IBinder binder, ILogger log)
{
    log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    foreach (var subscription in existingSubscriptions)
    {
        // binding in code to allow dynamic identity
        var subscriptionsToRefresh = await binder.BindAsync<IAsyncCollector<string>>(
            new GraphWebhookSubscriptionAttribute()
            {
                Action = GraphWebhookSubscriptionAction.Refresh,
                Identity = TokenIdentityMode.UserFromId,
                UserId = subscription.UserId
            }
        );
        {
            log.LogInformation($"Refreshing subscription {subscription.Id}");
            await subscriptionsToRefresh.AddAsync(subscription.Id);
        }
    }
}

public class UserSubscription {
    public string UserId {get; set;}
    public string Id {get; set;}
}