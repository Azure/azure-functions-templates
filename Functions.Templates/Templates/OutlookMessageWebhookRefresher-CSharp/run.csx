using System;

public static void Run(TimerInfo myTimer, string[] existingSubscriptions, ICollector<string> subscriptionsToRefresh, TraceWriter log)
{
    // This template uses application permissions and requires consent from an Azure Active Directory admin.
    // See https://go.microsoft.com/fwlink/?linkid=858780
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
	foreach (var subscription in existingSubscriptions)
	{
		log.Info($"Refreshing subscription {subscription}");
        subscriptionsToRefresh.Add(subscription);
    }
}