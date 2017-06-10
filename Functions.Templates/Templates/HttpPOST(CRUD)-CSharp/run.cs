#if (portalTemplates)
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ICollector<Person> outTable, TraceWriter log)
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
    public static class HttpPostCRUDCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")]HttpRequestMessage req, [Table("TableNameValue", Connection = "ConnectionValue")]ICollector<Person> outTable, TraceWriter log)
#endif
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string name = data?.name;

            if (name == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            outTable.Add(new Person()
            {
                PartitionKey = "Functions",
                RowKey = Guid.NewGuid().ToString(),
                Name = name
            });
            return req.CreateResponse(HttpStatusCode.Created);
        }

        public class Person : TableEntity
        {
            public string Name { get; set; }
        }
#if (vsTemplates)
    }
}
#endif