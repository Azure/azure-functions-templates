using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.CommonDataService;
using Microsoft.CommonDataService.Configuration;
using Microsoft.CommonDataService.ServiceClient.Security;
using Microsoft.CommonDataService.ServiceClient.Security.Credentials;

internal class SecurityHelper
{
    public static Task<Client> GetClientFromConfiguration(HttpRequestMessage requestMessage, TraceWriter logger)
    {
        var settings = new ConnectionSettings
        {
            Tenant = EnvironmentValues.Tenant,
            EnvironmentId = EnvironmentValues.EnvironmentId,
            Credentials = new UserImpersonationCredentialsSettings()
            {
                ApplicationId = EnvironmentValues.ApplicationId,
                ApplicationSecret = EnvironmentValues.ApplicationSecret
            }
        };

        return settings.CreateClient(requestMessage, new TraceWriterTelemetryBridge(logger));
    }
}