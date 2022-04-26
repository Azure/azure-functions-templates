// run 'npm install @azure/functions-authentication-events' from the project root folder

import { AzureFunction, Context } from "@azure/functions"
import { FailedRequest, RequestStatus, IEventResponse } from "@azure/functions-authentication-events"

import { TokenIssuanceStartRequest } from "@azure/functions-authentication-events/tokenIssuanceStart/preview_10_01_2021";
import { ProvideClaimsForToken, Claim } from "@azure/functions-authentication-events/tokenIssuanceStart/preview_10_01_2021/actions";

const OnTokenIssuanceStart: AzureFunction = async (context: Context, onTokenIssuanceStartRequest: TokenIssuanceStartRequest): Promise<IEventResponse> => {
    try {
        //Is the request successful and did the token validation pass.
        if (onTokenIssuanceStartRequest.requestStatus === RequestStatus.Successful) {
            //Fetch information about user from external data store

            //Add new claims to the token's response
            onTokenIssuanceStartRequest.response.actions.push(new ProvideClaimsForToken(
                [
                    new Claim("DateOfBirth", "01/01/2000"),
                    new Claim("CustomRoles", ["Writer", "Editor"])
                ]
            ));
        } else {
            //If the request failed for any reason, i.e. Token validation, output the failed request status
            context.log.error(onTokenIssuanceStartRequest.statusMessage);
        }

        return onTokenIssuanceStartRequest.response;
    } catch (e) {
        return FailedRequest.handle(e);
    }
};



export default OnTokenIssuanceStart;

