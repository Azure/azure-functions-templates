using System;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;

FunctionsDebugger.Enable();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
