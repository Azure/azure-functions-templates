#if (portalTemplates)
#r "O365Extensions"
#r "Microsoft.Graph"
using System;
using Microsoft.Graph
using O365Extensions;

public static void Run(Message msg, TraceWriter log)
#endif
#if (vsTemplates)

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Graph;
using O365Extensions;

namespace Company.Function
{
    public static class MSGraphWebhookCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static void Run([GraphWebhookTrigger(Type = "#Microsoft.Graph.Message")]Message msg, TraceWriter log)
#endif
        {
            log.Info($"Received email with subject: {msg.Subject} at {msg.SentDateTime.ToString()}");
        }
#if (vsTemplates)
    }
}
#endif