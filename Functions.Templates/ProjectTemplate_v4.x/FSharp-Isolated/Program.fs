open Microsoft.Extensions.Hosting

[<EntryPoint>]
let main args =
    let host =
        HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build()

    host.Run()
    0
