using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Kusto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function 
{
  public static class KustoOutputBindingCSharp 
  {
    // Visit https://github.com/Azure/Webjobs.Extensions.Kusto/tree/main/samples/samples-csharp#kustoattribute-for-output-bindings
    // KustoInputBinding sample 
    // Execute queries against the ADX cluster.
    // Add `KustoConnectionString` to the local.settings.json
    [FunctionName("KustoOutputBindingCSharp")]
    public static async Task <CreatedResult> Run(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
      [Kusto(Database: "db", // The database to ingest the data into , e.g. functionsdb
            TableName = "TargetTable", // Table to ingest data into, e.g. Storms
            Connection = "KustoConnectionString")] IAsyncCollector<Item> output, ILogger log) 
    {

      log.LogInformation("C# HTTP trigger with Kusto Output Binding function processed a request.");

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      Item item = JsonConvert.DeserializeObject <Item> (requestBody) ?? new Item {
          ItemID = 1,
          ItemName = "Item-1",
          ItemCost = 2.03
      };
      await output.AddAsync(item);
      return new CreatedResult(req.Path, item);
    }
  }
  public class Item
  {
      public long ItemID { get; set; }
      public string ItemName { get; set; }
      public double ItemCost { get; set; }
  }
}