#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"
#r "Newtonsoft.Json"

using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.AspNetCore.Mvc;

public static async Task<IActionResult> Run(
    HttpRequest req,
    IDurableEntityClient client,
    string entityKey)
{
    var entityId = new EntityId("Counter", entityKey);

    if (req.Method.Equals("POST"))
    {
        await client.SignalEntityAsync(entityId, "add", 1);
        return new OkObjectResult("Added to the entity");
    }

    EntityStateResponse<JToken> stateResponse = await client.ReadEntityStateAsync<JToken>(entityId);
    return new OkObjectResult(stateResponse);
}