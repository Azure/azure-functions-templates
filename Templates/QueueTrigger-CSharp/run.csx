using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Verbose($"CSharp Queue trigger function processed: {myQueueItem}");
}