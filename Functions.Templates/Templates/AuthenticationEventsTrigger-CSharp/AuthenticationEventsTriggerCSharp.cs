using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.Framework;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.Actions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    /// <summary>Example functions for token augmentation</summary>
    public static class AuthenticationEventFunctions
    {
        /// <summary>The entry point for the Azure Function</summary>
        /// <param name="request">Strongly Typed request data for a token issuance start request</param>
        /// <param name="log">Logger</param>
        /// <returns>The augmented token response or error.</returns>
        [FunctionName("onTokenIssuanceStart")]
        public async static Task<AuthenticationEventResponse> OnTokenIssuanceStartEvent(
            [AuthenticationEventsTrigger] TokenIssuanceStartRequest request, ILogger log)
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
    }
}
