#### Microsoft Graph webhook subscription input binding

This binding allows you to retrieve the list of subscriptions managed by this function app. The binding reads from function app storage, and does not reflect other subscriptions created from outside the app.

#### Configuring a Microsoft Graph webhook subscription input binding

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the mail message.
- `type`: *(required)* must be set to `graphWebhookSubscription`.
- `direction`: *(required)* must be set to `in`.
- `filter`: If set to `userFromRequest`, then the binding will only retrieve subscriptions owned by the calling user ( valid only with HTTP trigger).

#### Using a Microsoft Graph webhook subscription input binding from code

The binding exposes the following types to .NET functions:
- string[]
- Custom object type arrays
- Newtonsoft.Json.Linq.JObject[]
- Microsoft.Graph.Subscription[]