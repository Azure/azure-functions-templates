#### Settings for SendGrid output binding

- `name` : Variable name used in function code for the SendGrid Mail Object.
- `type` : must be set to *"sendGrid"*.
- `apiKey` : The name of the app setting containing your SendGrid developer API key.
- `to` : Email address to which the message should be sent. Can be of the form `user@host.com` or `Display Name <user@host.com>`. If not provided here, the value can be specified in code.
- `from` : Email address to which the message should be sent. Can be of the form `user@host.com` or `Display Name <user@host.com>`. If not provided here, the value can be specified in code.
- `subject` : Subject line to use for the message. If not provided here, the value can be specified in code.
- `text` : Text body to use for the message. If not provided here, the value can be specified in code. 
- `direction` : must be set to *"out"*. 

#### SendGrid output code example for a JavaScript queue trigger

```javascript
var util = require('util');

// The 'From' andd 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }

module.exports = function (context, order) {
    context.log('JavaScript queue trigger function processed order', order.orderId);

    context.done(null, {
        message: {
            subject: util.format('Thanks for your order (#%d)!', order.orderId),
            content: [{
                type: 'text/plain',
                value: util.format("%s, your order (%d) is being processed!", order.customerName, order.orderId)
            }]
        }
    });
}
```

#### SendGrid output code example for a C# queue trigger

```csharp
#r "SendGrid"

using System;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs.Host;

// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
public static Mail Run(Order order, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed order: {order.OrderId}");
    
    Mail message = new Mail()
    {
        Subject = $"Thanks for your order (#{order.OrderId})!"
    };

    Content content = new Content
    {
        Type = "text/plain",
        Value = $"{order.CustomerName}, your order ({order.OrderId}) is being processed!"
    };

    message.AddContent(content);    
    return message;
}

public class Order
{
    public string OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
}
```

#### Sample Queue message for the above C# and Javascript code examples

```json
{
    "OrderId": 12345,
    "CustomerName": "Joe Schmoe", 
    "CustomerEmail": "joeschmoe@foo.com" 
}
```