#### Microsoft Graph webhook trigger

This trigger allows a function to react to an incoming webhook from the Microsoft Graph. Each instance of this trigger can react to one Microsoft Graph resource type.

#### Configuring a Microsoft Graph webhook trigger

The binding supports the following properties:

- `name`: *(required)* the variable name used in function code for the mail message.
- `type`: *(required)* must be set to `graphWebhook`.
- `direction`: *(required)* must be set to `trigger`.
`resourceType`: *(required)* the graph resource for which this function should respond to webhooks. Can be one of the following values:
   - `#Microsoft.Graph.Message`: changes made to Outlook messages.
   - `#Microsoft.Graph.DriveItem`: changes made to OneDrive root items.
   - `#Microsoft.Graph.Contact`: changes made to personal contacts in Outlook.
   - `#Microsoft.Graph.Event`: changes made to Outlook calendar items.

> Note
> A function app can only have one function which is registered against a given `resourceType` value.

#### Using a Microsoft Graph webhook trigger from code

The binding exposes the following types to .NET functions:
- Microsoft Graph SDK types relevant to the resource type, e.g., Microsoft.Graph.Message, Microsoft.Graph.DriveItem
- Custom object types (using structural model binding)