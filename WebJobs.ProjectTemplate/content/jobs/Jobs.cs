using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace MyWebJobsProject
{
    public class Jobs
    {
        public static void ListenOnQueue([QueueTrigger("testqueue")] string message, TraceWriter log)
        {
            log.Info($"Hello {message}");
        }
    }
}