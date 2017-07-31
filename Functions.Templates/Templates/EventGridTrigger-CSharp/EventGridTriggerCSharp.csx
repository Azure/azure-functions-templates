#r "Microsoft.Azure.WebJobs.Extensions.EventGrid"
using Microsoft.Azure.WebJobs.Extensions.EventGrid;

public static void Run(EventGridEvent myBlob, TraceWriter log)
{
    log.Info(myBlob.ToString());
}