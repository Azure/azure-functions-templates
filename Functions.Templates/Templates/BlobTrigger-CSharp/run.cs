#if (portalTemplates)
public static void Run(Stream myBlob, string name, TraceWriter log)
#endif
#if (vsTemplates)
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Company.Function
{
    public static class BlobTriggerCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static void Run([BlobTrigger("PathValue/{name}", Connection = "ConnectionValue")]Stream myBlob, string name, TraceWriter log)
#endif
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
#if (vsTemplates)
    }
}
#endif