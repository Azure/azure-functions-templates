#if (portalTemplates)
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(Person person, CloudTable outTable, TraceWriter log)
#endif
#if (vsTemplates)
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace Company.Function
{
    public static class HttpPutCRUDCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "put")]Person person, [Table("TableNameValue", Connection = "ConnectionValue")]CloudTable outTable, TraceWriter log)
#endif
        {
            if (string.IsNullOrEmpty(person.Name))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("A non-empty Name must be specified.")
                };
            };

            log.Info($"PersonName={person.Name}");

            TableOperation updateOperation = TableOperation.InsertOrReplace(person);
            TableResult result = outTable.Execute(updateOperation);
            return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
        }

        public class Person : TableEntity
        {
            public string Name { get; set; }
        }
#if (vsTemplates)
    }
}
#endif