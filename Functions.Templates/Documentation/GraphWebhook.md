#### Settings for MS Graph GraphWebhook Binding
The settings specify the following properties:

- `name` : The variable name used in function code for the GraphWebhook handler. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *GraphWebhook*.

#### C# Example code
```csharp
using O365Extensions;

// Update existing Microsoft Graph subscriptions
public static async Task Run(TimerInfo timer, TraceWriter log, GraphWebhook handler)
{
	await handler.RefreshAllAsync();
}
```

#### Supported types

Use the data type *GraphWebhook* to modify subscriptions (refresh, delete, etc.).
