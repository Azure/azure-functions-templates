#Import the event and related version modules.
from azure.functions.authentication_events.token_issuance_start import *
from azure.functions.authentication_events.token_issuance_start.actions import ProvideClaimsForToken, Claim

def main(onTokenIssuanceStartRequest: TokenIssuanceStartRequest) -> TokenIssuanceStartResponse:
    try:
        # Is the request successful and did the token validation pass.
        if onTokenIssuanceStartRequest.requestStatus == RequestStatus.Successful:
            #Fetch information about user from external data store

            # Add new claims to the token's response
            onTokenIssuanceStartRequest.response.actions.append(ProvideClaimsForToken(
                [
                    Claim("DateOfBirth", "01/01/2000"),
                    Claim("CustomRoles", ["Writer", "Editor"])
                ]
            ))

        return onTokenIssuanceStartRequest.response
    except Exception as e:
        return FailedRequest.handle(e)
