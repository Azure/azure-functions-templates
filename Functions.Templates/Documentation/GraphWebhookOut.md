#### Microsoft Graph webhook subscription output binding

This binding allows you to create, delete, and refresh webhook subscriptions in the Microsoft Graph.

#### Configuring a Microsoft Graph webhook subscription output binding

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the mail message.
- `type`: *(required)* must be set to `graphWebhookSubscription`.
- `direction`: *(required)* must be set to `out`.
- `identity`: *(required)* The identity that will be used to perform the action. Can be one of the following values:
  - userFromRequest: Only valid with HTTP trigger. Uses the identity of the calling user.
  - userFromId: Uses the identity of a previously logged-in user with the specified ID. See the `userId` property.
  - userFromToken: Uses the identity represented by the specified token. See the `userToken` property.
  - clientCredentials: Uses the identity of the function app.
- `userId`: Needed if and only if `identity` is set to `userFromId`. A user principal ID associated with a previously logged-in user.
- `userToken`: Needed if and only if `identity` is set to `userFromToken`. A token valid for the function app.
- `action`: *(required)* specifies the action the binding should perform. Can be one of the following values:
  - `create`: Registers a new subscription.
  - `delete`: Deletes a specified subscription.
  - `refresh`: Refreshes a specified subscription to keep it from expiring.
- `subscriptionResource`: Needed if and only if the _action_ is set to `create`. Specifies the Microsoft Graph resource that will be monitored for changes.
- `changeTypes`: Needed if and only if the _action_ is set to `create`. Indicates the type of change in the subscribed resource that will raise a notification. The supported values are: `created`, `updated`, `deleted`. Multiple values can be combined using a comma-separated list.

#### Using a Microsoft Graph webhook subscription output binding from code

The binding exposes the following types to .NET functions:
- string
- Microsoft.Graph.Subscription