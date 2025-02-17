open Microsoft.Azure.Functions.Worker
#if (!FrameworkShouldUseV1Dependencies)
open Microsoft.Azure.Functions.Worker.Builder
#endif
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

#if (FrameworkShouldUseV1Dependencies)
[<EntryPoint>]
let main args =
    let host =
        HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureServices(fun services ->
                services.AddApplicationInsightsTelemetryWorkerService()
                |> ignore

                services.ConfigureFunctionsApplicationInsights() |> ignore)
            .Build()

    // If using the Cosmos DB, Blob or Tables extension, you need to configure the extensions manually using the extension methods below.
    // Learn more about this here: https://go.microsoft.com/fwlink/?linkid=2245587
    // ConfigureFunctionsWebApplication(fun (context: HostBuilderContext) (appBuilder: IFunctionsWorkerApplicationBuilder) ->
    //     appBuilder.ConfigureCosmosDBExtension() |> ignore
    //     appBuilder.ConfigureBlobStorageExtension() |> ignore
    //     appBuilder.ConfigureTablesExtension() |> ignore
    // ) |> ignore

    host.Run()
    0
#else
[<EntryPoint>]
let main args =
    let builder = FunctionsApplication.CreateBuilder(args)

    builder.ConfigureFunctionsWebApplication() |> ignore

    builder.Services
        .AddApplicationInsightsTelemetryWorkerService()
        .ConfigureFunctionsApplicationInsights() 
    |> ignore
            
    // If using the Cosmos DB, Blob or Tables extension, you need to configure the extensions manually using the extension methods below.
    // Learn more about this here: https://go.microsoft.com/fwlink/?linkid=2245587
    // builder.ConfigureCosmosDBExtension() |> ignore
    // builder.ConfigureBlobStorageExtension() |> ignore
    // builder.ConfigureTablesExtension() |> ignore

    builder.Build().Run()
    0
#endif