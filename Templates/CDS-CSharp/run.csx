// Usage of this template is subject to license terms of the 
// Microsoft Common Data Service Software Development Kit (https://go.microsoft.com/fwlink/?linkid=842862).

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
    internal const string Tenant = "[[Replace with AAD tenant for CDS value]]";
    internal const string EnvironmentId = "[[Replace with PowerApps environment ID value]]";
    internal const string ApplicationId = "[[Replace with Function application ID value]]";
    internal const string ApplicationSecret = "[[Replace with Function application secret value]]";
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
        // Query product categories for Surfaces and Phones
        var query = client.GetRelationalEntitySet<ProductCategory>()
            .CreateQueryBuilder()
            .Where(pc => pc.Name == "Surface" || pc.Name == "Phone")
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