// Usage of this template is subject to license terms of the 
// Microsoft Common Data Service Software Development Kit (https://go.microsoft.com/fwlink/?linkid=842862).

#load ".\telemetrybridge.csx"

using Microsoft.CommonDataService;
using Microsoft.CommonDataService.CommonEntitySets;
using Microsoft.CommonDataService.Configuration;
using Microsoft.CommonDataService.ServiceClient.Security;
using Microsoft.CommonDataService.ServiceClient.Security.Credentials;
using System;
using System.Collections.Generic;
using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# CDS trigger function processed a request. RequestUri={req.RequestUri}");

    dynamic data = await req.Content.ReadAsAsync<object>();
    string name = data?.name;

    if (name == null)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
    }

    var connection = new ConnectionSettings
    {
        Tenant = "common",
        EnvironmentId = "[[Replace with PowerApps environment ID value]]",
        Credentials = new UserImpersonationCredentialsSettings
        {
            ApplicationId = Environment.GetEnvironmentVariable("WEBSITE_AUTH_CLIENT_ID"),
            ApplicationSecret = Environment.GetEnvironmentVariable("WEBSITE_AUTH_CLIENT_SECRET")
        }
    };

    using (var client = await connection.CreateClient(req, new TraceWriterTelemetryBridge(log)))
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

        // Delete any Surfaces and Phones
        var deleteExecutor = client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional);
        foreach (var entry in queryResult.Result)
        {
            deleteExecutor.DeleteWithoutConcurrencyCheck(entry);
        }
        deleteExecutor.ExecuteAsync().Wait();

        // Insert Surface and Phone product lines
        var surfaceCategory = new ProductCategory() { Name = "Surface", Description = "Surface product line" };
        var phoneCategory = new ProductCategory() { Name = "Phone", Description = "Phone product line" };
        await client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional)
            .Insert(surfaceCategory)
            .Insert(phoneCategory)
            .ExecuteAsync();

        // Query for Surface and Phone Product lines
        query = client.GetRelationalEntitySet<ProductCategory>()
            .CreateQueryBuilder()
            .Where(pc => pc.Name == name)
            .OrderByAscending(pc => new object[] { pc.CategoryId })
            .Project(pc => pc.SelectField(f => f.CategoryId).SelectField(f => f.Name).SelectField(f => f.Description));

        await client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional)
            .Query(query, out queryResult)
            .ExecuteAsync();

        // Update all selected Product Lines with description
        var updateExecutor = client.CreateRelationalBatchExecuter(RelationalBatchExecutionMode.Transactional);
        foreach (var entry in queryResult.Result)
        {
            log.Info($"Updateing '{entry.Name}'.");
            var updateProductCategory = client.CreateRelationalFieldUpdates<ProductCategory>();
            string updatedDescription = $"{DateTime.Now.ToString()} - Updated '{entry.Name}'";
            updateProductCategory.Update(pc => pc.Description, updatedDescription);

            updateExecutor.Update(entry, updateProductCategory);
        }
        await updateExecutor.ExecuteAsync();

        log.Info($"C# CDS trigger function completed.");
        return req.CreateResponse(HttpStatusCode.OK);
    }
}