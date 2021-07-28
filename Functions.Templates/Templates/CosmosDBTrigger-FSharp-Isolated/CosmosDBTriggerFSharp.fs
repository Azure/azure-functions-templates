namespace Company.Function

open System
open System.Collections.Generic
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module CosmosDBTriggerFSharp =
    type MyDocument =
        { Id: string
          Text: string
          Number: int
          Boolean: bool }

    [<Function("CosmosDBTriggerFSharp")>]
    let run
        (
            [<CosmosDBTrigger(databaseName = "DatabaseValue",
                              collectionName = "CollectionValue",
                              ConnectionStringSetting = "ConnectionValue",
                              LeaseCollectionName = "leases")>] input: IReadOnlyList<MyDocument>,
            context: FunctionContext
        ) =
        let logger =
            context.GetLogger "CosmsoDBTriggerFSharp"

        if not (isNull input) && input.Count > 0 then
            log.LogInformation(sprintf "Documents modified %d" input.Count)
            log.LogInformation("First document Id " + input.[0].Id)
