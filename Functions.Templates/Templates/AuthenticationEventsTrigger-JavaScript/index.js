module.exports = async function (context, onTokenIssuanceStartRequest) {
  //Is the request successful and did the token validation pass.
  if (onTokenIssuanceStartRequest.requestStatus === "Successful") {
    //Fetch information about user from external data store

    //Add new claims to the token's response
    onTokenIssuanceStartRequest.response.actions.push({
      actionType: "microsoft.graph.provideClaimsForToken",
      claims: {
        DateOfBirth: "01/01/2000",
        CustomRoles: ["Writer", "Editor"],
      },
    });
  } else {
    //If the request failed for any reason, i.e. Token validation, output the failed request status
    context.log.error(onTokenIssuanceStartRequest.statusMessage);
  }

  return onTokenIssuanceStartRequest.response;
};
