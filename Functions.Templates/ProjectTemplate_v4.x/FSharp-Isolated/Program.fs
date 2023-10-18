open Microsoft.Extensions.Hosting
open Microsoft.Azure.Functions.Worker

[<EntryPoint>]
let main args =
    let host =
        HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services => {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
            })
            .Build()

        // If using the Cosmos, Blob or Tables extension, you will need configure the extensions manually using the extension methods below.
        // Learn more about this here: https://go.microsoft.com/fwlink/?linkid=2245587
        // ConfigureFunctionsWorkerDefaults(fun (context: HostBuilderContext) (appBuilder: IFunctionsWorkerApplicationBuilder) ->
        //     appBuilder.ConfigureCosmosDBExtension() |> ignore
        //     appBuilder.ConfigureBlobStorageExtension() |> ignore
        //     appBuilder.ConfigureTablesExtension() |> ignore
        // ) |> ignore

    host.Run()
    0
