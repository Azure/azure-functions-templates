## Settings for Azure Notification Hub output binding

The settings provide the following properties:

- `name` : Variable name used in function code for the notification hub message.
- `type` : must be set to *"notificationHub"*.
- `tagExpression` : Tag expressions allow you to specify that notifications be delivered to a set of devices who have registered to receive notifications that match the tag expression.  For more information, see [Routing and tag expressions](https://azure.microsoft.com/en-us/documentation/articles/notification-hubs-tags-segment-push-message/).
- `hubName` : Name of the notification hub resource in the Azure portal.
- `connection` : This connection string must be an **Application Setting** connection string set to the *DefaultFullSharedAccessSignature* value for your notification hub.
- `direction` : must be set to *"out"*. 

## Azure Notification Hub connection string setup

To use a Notification hub output binding you must configure the connection string for the hub. You can do this on the *Integrate* tab by simply selecting your notification hub or creating a new one. 

You can also manually add a connection string for an existing hub by adding a connection string for the *DefaultFullSharedAccessSignature* to your notification hub. This connection string provides your function access permission to send notification messages. The *DefaultFullSharedAccessSignature* connection string value can be accessed from the **keys** button in the main blade of your notification hub resource in the Azure portal. To manually add a connection string for your hub, use the following steps: 

1. On the **Function app** blade of the Azure portal, click **Open Application Settings**.
2. Scroll down to the **Connection strings** section, and add an named entry for *DefaultFullSharedAccessSignature* value for you notification hub. Change the type to **Custom**.
3. Reference your connection string name in the output bindings. Similar to **MyHubConnectionString** used in the example above.

## Azure Notification Hub code example for a JavaScript timer trigger 

This example sends a notification for a [template registration](https://azure.microsoft.com/en-us/documentation/articles/notification-hubs-templates-cross-platform-push-messages/) that contains `location` and `message`.

```javascript
module.exports = function (context, myTimer) {
    var timeStamp = new Date().toISOString();
    
    if(myTimer.isPastDue)
    {
        context.log('JavaScript is running late!');
    }
    context.log('JavaScript timer trigger function ran!', timeStamp);  
    context.bindings.notification = {
        location: "Redmond",
        message: "Hello from JavaScript!"
    };
    context.done();
};
```

## Azure Notification Hub code example for a C# queue trigger

This example sends a notification for a [template registration](https://azure.microsoft.com/en-us/documentation/articles/notification-hubs-templates-cross-platform-push-messages/) that contains `message`.

```csharp
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
    
public static void Run(string myQueueItem,  out IDictionary<string, string> notification, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    notification = GetTemplateProperties(myQueueItem);
}
    
private static IDictionary<string, string> GetTemplateProperties(string message)
{
    Dictionary<string, string> templateProperties = new Dictionary<string, string>();
    templateProperties["message"] = message;
    return templateProperties;
}
```

This example sends a notification for a [template registration](https://azure.microsoft.com/en-us/documentation/articles/notification-hubs-templates-cross-platform-push-messages/) that contains `message` using a valid JSON string.

```csharp    
public static void Run(string myQueueItem,  out string notification, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    notification = "{\"message\":\"Hello from C#. Processed a queue item!\"}";
}
```

## Azure Notification Hub queue trigger C# code example using Notification type

This example shows how to use the `Notification` type that is defined in the [Microsoft Azure Notification Hubs Library](https://www.nuget.org/packages/Microsoft.Azure.NotificationHubs/). In order to use this type, and the library, you must upload a *project.json* file for your function app. The project.json file is a JSON text file which will look similar to the follow:
```json
{
    "frameworks": {
    ".NETFramework,Version=v4.6": {
        "dependencies": {
        "Microsoft.Azure.NotificationHubs": "1.0.4"
        }
    }
    }
}
```

For more information on uploading your project.json file, see [uploading a project.json file](https://azure.microsoft.com/en-us/documentation/articles/functions-reference-csharp/#_how-to-upload-a-projectjson-file).

Example code:

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
    
public static void Run(string myQueueItem,  out Notification notification, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    notification = GetTemplateNotification(myQueueItem);
}
private static TemplateNotification GetTemplateNotification(string message)
{
    Dictionary<string, string> templateProperties = new Dictionary<string, string>();
    templateProperties["message"] = message;
    return new TemplateNotification(templateProperties);
}
```