 #r "Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents"

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.Framework;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.Actions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public async static Task<AuthenticationEventResponse> Run(TokenIssuanceStartRequest request, ILogger log)
{
    try
    {
        //Is the request successful and did the token validation pass.
        if (request.RequestStatus == RequestStatusType.Successful)
        {
            // Fetch information about user from external data store

            //Add new claims to the token's response
            request.Response.Actions.Add(new ProvideClaimsForToken(
                                            new TokenClaim("DateOfBirth", "01/01/2000"),
                                            new TokenClaim("CustomRoles", "Writer", "Editor")
                                        ));
        }
        else
        {
            //If the request failed for any reason, i.e. Token validation, output the failed request status
            log.LogInformation(request.StatusMessage);
        }
        return await request.Completed();
    }
    catch (Exception ex)
    {
        return await request.Failed(ex);
    }
}