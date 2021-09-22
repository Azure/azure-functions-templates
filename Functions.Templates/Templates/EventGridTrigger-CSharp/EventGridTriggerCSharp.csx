#r "Microsoft.Azure.EventGrid"
using Microsoft.Azure.EventGrid.Models;

public void Run(EventGridEvent eventGridEvent)
{
    log.LogInformation(eventGridEvent.Data.ToString());
}
