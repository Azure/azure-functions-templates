#### Settings for MS Graph GraphWebhook Binding
The settings specify the following properties:

- `name` : The variable name used in function code for the GraphWebhook handler. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *GraphWebhook*.

#### Example function.json
```json
{
  "bindings": [
    {
      "type": "timerTrigger",
      "direction": "in",
      "name": "timer",
      "schedule": "0 15 2 * * *"
    },
    {
      "type": "GraphWebhook",
      "name": "g",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
#r "O365Extensions"

using System;
using System.Net;
using O365Extensions;

/*
 * MS Graph subscriptions expire, by default, every 3 days
 * This function runs every morning at 2:15 AM
 * Automatically refreshes subscriptions associated with this Function App
 */
public static async Task Run(TimerInfo timer, TraceWriter log, GraphWebhook g)
{
    await g.RefreshAllAsync();
}
```

#### Supported types

Use the data type *GraphWebhook* to modify subscriptions (refresh, delete) associated with this Function App.
