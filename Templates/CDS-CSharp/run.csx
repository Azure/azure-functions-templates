#load ".\SecurityHelper.csx"
#load ".\TraceWriterTelemetryBridge.csx"

using Microsoft.CommonDataService;
using Microsoft.CommonDataService.CommonEntitySets;
using Microsoft.CommonDataService.Configuration;
using Microsoft.CommonDataService.ServiceClient.Security;
using Microsoft.CommonDataService.ServiceClient.Security.Credentials;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

class EnvironmentValues
{
    internal const string Tenant = "<REPLACE WITH YOUR TENANT INFORMATION>";
    internal const string EnvironmentId = "<REPLACE WITH YOUR ENVEIRONMENT ID INFORMATION>";
    internal const string ApplicationId = "<REPLACE WITH YOUR APPLICATION ID INFORMATION>";
    internal const string ApplicationSecret = "<REPLACE WITH YOUR APPLICATION SECRET INFORMATION>";
    internal const string RedirectUri = "<REPLACE WITH YOUR REDIRECT URI INFORMATION>";
}

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    if (req.Headers.Authorization == null)
    {
        return req.CreateResponse(HttpStatusCode.Forbidden);
    }

    using (var client = await SecurityHelper.GetClientFromConfiguration(req, log))
    {
        // Query product categories for Surface
        // TODO: pick this from req parameters
        var query = client.GetRelationalEntitySet<ProductCategory>()
            .CreateQueryBuilder()
            .Where(pc => pc.Name == "Surface")
            .Project(pc => pc.SelectField(f => f.CategoryId).SelectField(f => f.Name));

        OperationResult<IReadOnlyList<ProductCategory>> queryResult = null;
        client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional)
            .Query(query, out queryResult)
            .ExecuteAsync().Wait();
        ;

        // Delete any Surfaces and Phones
        var deleteExecutor = client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional);
        foreach (var entry in queryResult.Result)
        {
            deleteExecutor.DeleteWithoutConcurrencyCheck(entry);
        }
        deleteExecutor.ExecuteAsync().Wait();

        // Insert Surface and Phone product lines
        var surfaceCategory = new ProductCategory() { Name = "Surface", Description = "Surface produce line" };
        var phoneCategory = new ProductCategory() { Name = "Phone", Description = "Phone produce line" };
        client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional)
            .Insert(surfaceCategory)
            .Insert(phoneCategory)
            .ExecuteAsync().Wait();

        log.Info($"C# HTTP trigger function completed.");
        return req.CreateResponse(HttpStatusCode.OK);
    }
}