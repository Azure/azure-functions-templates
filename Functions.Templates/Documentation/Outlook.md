### MS Graph Outlook

#### Summary
This output binding can be used to send email(s) to recipient(s). The sender will be the email account associated with the Principal ID or ID Token.

#### Settings
The settings specify the following properties.

- `name` : The variable name used in function code for the Outlook message. 
- `direction` : Must be set to *out*. 
- `Type` : Must be set to *Outlook*.
- `PrincipalId` : Should be set to either an app setting containing the Principal ID/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal ID/OID
- `idToken` : Should be set to an expression that evaluates to an ID token. Either Principal ID or ID token must be set, but not both.

#### Example function.json
```json
{
  "bindings": [
    {
      "type": "timerTrigger",
      "direction": "in",
      "name": "timer",
      "schedule": "0 45 9 * * *"
    },
    {
      "type": "outlook",
      "name": "email",
      "PrincipalId": "Identity.alias",
      "direction": "out"
    }
  ],
  "disabled": false
}
```
#### Language Support

##### C#

###### Send one email to one person
```csharp
// Read Excel table & OneDrive
public static void Run(TimerInfo myTimer, TraceWriter log, ICollector<Message> email)
{
   
    var msg = new Message {
        Subject = "Subject!",
        Body = new ItemBody {
            Content = file,
            ContentType = BodyType.Html
        },
        ToRecipients = new Recipient[] {
            new Recipient {
                EmailAddress = new EmailAddress {
                    Address = "email@contoso.com",
                    Name = "Contoso"
                }
            }
        }
    };
    
    emails.Add(msg);
}

public class EmailRow {
    public string Name { get; set; }
    public string Email { get; set; }
}
```

###### Send one email to one person using JObjects
```csharp
public static void Run(TimerInfo timer, TraceWriter log, out JObject msg)
{
	msg = new JObject();
	msg["subject"] = "Greetings";
	msg["body"] = "Sent from Azure Functions";

	var recipient = new JObject();
	recipient["address"] = "test@microsoft.com",
	recipient["name"] = "Test Name",
	msg["recipient"] = recipient;		
}
```
###### Send one email to multiple people using JObjects
```csharp
public static void Run(TimerInfo timer, TraceWriter log, out JObject msg)
{
	msg = new JObject();
	msg["subject"] = "Greetings";
	msg["body"] = "Sent from Azure Functions";

	List<JObject> recipients = new List<JObject>();

	var recipient1 = new JObject();
	recipient1["address"] = "test@microsoft.com",
	recipient1["name"] = "Test Name",
	recipients.Add(recipient1);
	var recipient2 = new JObject();
	recipient2["address"] = "test@gmail.com",
	recipient2["name"] = "Test Name",
	recipients.Add(recipient2);

	msg["recipients"] = recipients;		
}
```

###### Supported types

Messages can be sent from any of the following types:

* Microsoft.Graph.Message
* JObject
* JSON formatted string matching Microsoft.Graph.Message class