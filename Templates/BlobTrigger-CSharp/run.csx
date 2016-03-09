using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

public static void Run(string myBlobTrigger, TraceWriter log)
{
    log.Verbose($"CSharp Blob trigger function processed: {myBlobTrigger}");
}