#if (portalTemplates)
#r "Microsoft.Azure.EventGrid"
#r "Newtonsoft.Json"

using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

public static void Run(EventGridEvent eventGridEvent, TraceWriter log)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class EventGridTriggerCSharp
    {
        [FunctionName("EventGridTriggerCSharp")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, TraceWriter log)
#endif
        {
            log.Info(JsonConvert.SerializeObject(eventGridEvent, Formatting.Indented));
        }
#if (vsTemplates)
    }
}
#endif