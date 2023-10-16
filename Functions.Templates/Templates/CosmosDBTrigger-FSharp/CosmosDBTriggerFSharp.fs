namespace Company.Function

open System.Collections.Generic
open Microsoft.Azure.Documents
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

module CosmosDBTriggerFSharp =
    type TodoItem =
        { id: string
          Description: string }

    [<FunctionName("CosmosDBTriggerFSharp")>]
    let run([<CosmosDBTrigger(databaseName="DatabaseValue", containerName="ContainerValue", Connection="ConnectionValue", LeaseContainerName="leases")>] input: IReadOnlyList<TodoItem>, log: ILogger) =
        if not(isNull input) && input.Count > 0 then
            log.LogInformation(sprintf "Documents modified %d" input.Count)
            log.LogInformation("First document Id " + input.[0].id)