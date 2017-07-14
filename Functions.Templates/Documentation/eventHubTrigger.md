#### Settings for Event Hub trigger binding

The settings for an Azure Event Hub trigger specifies the following properties:

- `type` : Must be set to *eventHubTrigger*.
- `name` : The variable name used in function code for the event hub message. 
- `direction` : Must be set to *in*. 
- `path` : The name of the event hub.
- `connection` : The name of an app setting that contains the connection string to the namespace that the event hub resides in. Copy this connection string by clicking the **Connection Information** button for the namespace, not the event hub itself.  This connection string must have at least read permissions to activate the trigger.
- `cardinality` : Cardinality of the trigger input. Choose 'One' if the input is a single message or 'Many' if the input is an array of messages. 'Many' is the default if unspecified.

#### Azure Event Hub trigger C# example
 
	using System;
	
	public static void Run(string myEventHubMessage, TraceWriter log)
	{
	    log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
	}

#### Azure Event Hub trigger JavaScript example

	module.exports = function (context, eventHubMessages) {
		context.log(`JavaScript eventhub trigger function called for message array ${eventHubMessages}`);
		
		eventHubMessages.forEach(message => {
			context.log(`Processed message ${message}`);
		});

		context.done();
	};
