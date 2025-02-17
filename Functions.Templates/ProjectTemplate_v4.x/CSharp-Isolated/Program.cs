#if (FrameworkShouldUseV1Dependencies)
using Microsoft.Azure.Functions.Worker;
#endif
#if (NetCore && !FrameworkShouldUseV1Dependencies)
using Microsoft.Azure.Functions.Worker.Builder;
#endif
#if (FrameworkShouldUseV1Dependencies)
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Hosting;

#if NetFramework
namespace Company.FunctionApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                })
                .Build();

            host.Run();
        }
    }
}
#elseif FrameworkShouldUseV1Dependencies
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
#else
var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
#endif