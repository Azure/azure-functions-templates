namespace Company.Function

open System.Collections.Generic
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

module CosmosDBTriggerFSharp =
    type MyDocument =
        { id: string
          Text: string
          Number: int
          Boolean: bool }

    [<Function("CosmosDBTriggerFSharp")>]
    let run
        (
            [<CosmosDBTrigger(databaseName = "DatabaseValue",
                              containerName = "ContainerValue",
                              Connection = "ConnectionValue",
                              LeaseContainerName = "leases")>] input: IReadOnlyList<MyDocument> | null,
            context: FunctionContext
        ) =
        let logger =
            context.GetLogger "CosmosDBTriggerFSharp"

        match Option.ofObj input with
        | Some input when input.Count > 0 ->
            logger.LogInformation(sprintf "Documents modified %d" input.Count)
            logger.LogInformation("First document Id " + input.[0].id)
        | _ -> ()
