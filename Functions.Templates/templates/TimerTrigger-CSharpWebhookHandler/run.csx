#r "O365Extensions"

using System;
using System.Net;
using O365Extensions;

/*
 * MS Graph subscriptions expire, by default, every 3 days
 * This function runs every morning at 2:15 AM
 * Automatically refreshes subscriptions associated with this Function App
 */
public static async Task Run(TimerInfo timer, TraceWriter log, GraphWebhook g)
{
    await g.RefreshAllAsync();
}