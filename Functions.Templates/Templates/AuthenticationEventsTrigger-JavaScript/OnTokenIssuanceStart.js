module.exports = async function (context, onTokenIssuanceStartRequest) {
    //Is the request successful and did the token validation pass.
    if(onTokenIssuanceStartRequest.requestStatus === "Successful"){
        //Fetch information about user from external data store

        //Add new claims to the token's response
        onTokenIssuanceStartRequest.response.actions.push({
            'type': 'ProvideClaimsForToken',
            'claims': [
                {
                    'id': 'DateOfBirth',
                    'value': '01/01/2000'
                },
                {
                    'id': 'CustomRoles',
                    'value': [
                        'Writer',
                        'Editor'
                    ]
                }
            ]
        });
    } else {
        //If the request failed for any reason, i.e. Token validation, output the failed request status
        context.log.error(onTokenIssuanceStartRequest.statusMessage);
    }

    return onTokenIssuanceStartRequest.response;
}