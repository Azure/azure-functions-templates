open Microsoft.Extensions.Hosting
open Microsoft.Azure.Functions.Worker

[<EntryPoint>]
let main args =
    let host =
        HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build()

        // If using the Cosmos, Blob or Tables extension, you will need configure the extensions manually using the extension methods below:
        // ConfigureFunctionsWorkerDefaults(fun (context: HostBuilderContext) (appBuilder: IFunctionsWorkerApplicationBuilder) ->
        //     appBuilder.ConfigureCosmosDBExtension() |> ignore
        //     appBuilder.ConfigureBlobStorageExtension() |> ignore
        //     appBuilder.ConfigureTablesExtension() |> ignore
        // ) |> ignore

    host.Run()
    0
