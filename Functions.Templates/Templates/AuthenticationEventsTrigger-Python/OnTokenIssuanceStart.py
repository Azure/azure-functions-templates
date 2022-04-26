#Import the event and related version modules.
from azure.functions.authentication_events.token_issuance_start.preview_10_01_2021 import *
from azure.functions.authentication_events.token_issuance_start.preview_10_01_2021.actions import ProvideClaimsForToken

def main(onTokenIssuanceStartRequest: TokenIssuanceStartRequest) -> TokenIssuanceStartResponse:

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
