#### Settings for MS Graph Webhook Trigger

The settings specify the following properties.

- `name` : The variable name used in function code for the payloads. 
- `type` : Must be set to *GraphWebhookTrigger*.
- `direction` : Must be set to *in*. 
- `Resource` : Resource this function will subscribe to.
- `ChangeType` : Kinds of changes function subscribes to.
- `Type` : Corresponds directly to resource. Specifies how notifications will be transformed after being received. Current options: #Microsoft.Graph.Message, #Microsoft.Graph.DriveItem, #Microsoft.Graph.Contact, #Microsoft.Graph.Event

#### Additional Information
Currently, notifications from webhooks are processed at the **application** level and then distributed to the proper function within the app. This means that if multiple functions are subscribed to the same resource (e.g. two functions have GraphWebhookTrigger triggers that are being serialized to Microsoft.Graph.Message), **only one will actually be triggered**. This only applies to functions within the same Function App that are subscribed to the same resource.

If viewing from Portal, one must click 'Subscribe' in order to initialize subscription.

#### Example function.json
```json
{
  "bindings": [
    {
      "type": "GraphWebhookTrigger",
      "name": "msg",
      "Type": "#Microsoft.Graph.Message",
      "ChangeType": [
        "created"
      ],
      "Listen": "me/mailFolders('Inbox')/messages",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
#r "O365Extensions"
#r "Microsoft.Graph"

using System;
using Microsoft.Graph;
using O365Extensions;

public static void Run(Message msg, TraceWriter log)
{
    log.Info($"Received email with subject: {msg.Subject} at {msg.SentDateTime.ToString()}");
}

```

#### Supported types

Notifications can be deserialized to any of the following types:

* Microsoft.Graph.Message
* Microsoft.Graph.DriveItem
* Microsoft.Graph.Contact
* Microsoft.Graph.Event