#### Settings for MS Graph Webhook Trigger

The settings specify the following properties.

- `name` : The variable name used in function code for the payloads. 
- `type` : Must be set to *GraphWebhookTrigger*.
- `direction` : Must be set to *in*. 
- `Resource` : Resource this function will subscribe to.
- `ChangeType` : Kinds of changes function subscribes to.
- `Type` : Corresponds directly to resource. Specifies how notifications will be transformed after being received. Current options: #Microsoft.Graph.Message, #Microsoft.Graph.DriveItem, #Microsoft.Graph.Contact, #Microsoft.Graph.Event

#### Additional Information
If viewing from Portal, must click 'Subscribe' in order to initialize subscription.

#### C# Example code
```csharp
using Microsoft.Graph;

public static void Run(Message msg, TraceWriter log)
{
    log.Info($"C# Graph Webhook trigger processed email: {msg.Subject}");
}
```

#### Supported types

Notifications can be deserialized to any of the following types:

* Microsoft.Graph.Message
* Microsoft.Graph.DriveItem
* Microsoft.Graph.Contact
* Microsoft.Graph.Event