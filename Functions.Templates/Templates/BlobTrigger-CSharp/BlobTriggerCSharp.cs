#if (portalTemplates)
public static void Run(Stream myBlob, string name, ILogger log)
#endif
#if (vsTemplates)
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class BlobTriggerCSharp
    {
        [FunctionName("BlobTriggerCSharp")]
        public static void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")]Stream myBlob, string name, ILogger log)
#endif
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
#if (vsTemplates)
    }
}
#endif