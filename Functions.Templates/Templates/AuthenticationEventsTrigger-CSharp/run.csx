#r "Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents"

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.preview_10_01_2021;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.preview_10_01_2021.Actions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
public async static Task<IActionResult> OnTokenIssuanceStartEvent(TokenIssuanceStartRequest request, ILogger log)
{
    try
    {
        //Is the request successful and did the token validation pass.
        if (request.RequestStatus == RequestStatus.Successful)
        {
            //Fetch information about user from external data store

            //Add new claims to the token's response
            request.Response.Actions.Add(new ProvideClaimsForToken(
                                            new Claim("DateOfBirth", "01/01/2000"),
                                            new Claim("CustomRoles", "Writer", "Editor")
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
        //If anything goes wrong, return the error
        return request.Failed(ex.Message);
    }
}
  

