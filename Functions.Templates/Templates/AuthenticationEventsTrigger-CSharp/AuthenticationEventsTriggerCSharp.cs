using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents;
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.[onTokenIssuanceStartVersions];
using Microsoft.Azure.WebJobs.Extensions.AuthenticationEvents.TokenIssuanceStart.[onTokenIssuanceStartVersions].Actions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace  Company.Function
{
    /// <summary>Example functions for token augmentation</summary>
    public static class AuthenticationEventsTriggerCSharp
    {
        /// <summary>The entry point for the Azure Function</summary>
        /// <param name="request">Strongly Typed request data for a token issuance start request</param>
        /// <param name="log">Logger</param>
        /// <returns>The augmented token response or error.</returns>
        [FunctionName("AuthenticationEventsTriggerCSharp")]
        public async static Task<IActionResult> OnTokenIssuanceStartEvent(
            [AuthenticationEventTrigger(
                EventType = EventTypes.onTokenIssuanceStart,
                ApiSchemaVersion = Versions.TokenIssuanceStart_[onTokenIssuanceStartVersions])] TokenIssuanceStartRequest request, ILogger log)
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
    }
}
