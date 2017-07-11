#### Settings for MS Graph Outlook Binding
The settings specify the following properties.

- `name` : The variable name used in function code for the Outlook message. 
- `direction` : Must be set to *out*. 
- `Type` : Must be set to *Outlook*.
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal id/OID
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.

#### C# Example code
```csharp
// Send one email to one person using JObjects
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
```csharp
// Send one email to multiple people using JObjects
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
```csharp
// Send one email to one person using simple string array
public static void Run(TimerInfo timer, TraceWriter log, out string[] msg)
{
	msg = new string[3];
	msg[0] = "test@outlook.com";
	msg[1] = "Subject";
	msg[2] = "Body";
}
```


#### Supported types

[Output] Excel data can be deserialized to any of the following types:

* Microsoft.Graph.Message
* JObject
* string[]