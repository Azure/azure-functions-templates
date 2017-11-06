#if (portalTemplates)
/*
 * Before running this sample, please create a Durable Activity function (default name is "hello")
 */

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"
 
public static string Run(string name)
#endif
#if (vsTemplates)
using Microsoft.Azure.WebJobs;

namespace Company.Function
{
    public static class DurableFunctionsActivityCSharp
    {
        [FunctionName("DurableFunctionsActivityCSharp")]
        public static string SayHello([ActivityTrigger] string name)
#endif
        {
            return $"Hello {name}!";
        }
#if (vsTemplates)
    }
}
#endif