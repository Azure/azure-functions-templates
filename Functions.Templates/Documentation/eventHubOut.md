#### Settings for Event Hub output binding

The settings for an Azure Event Hub output binding specifies the following properties:

- `type` : Must be set to *eventHub*.
- `name` : The variable name used in function code for the event hub message. 
- `path` : The name of the event hub.
- `connection` : The name of an app setting that contains the connection string to the namespace that the event hub resides in. Copy this connection string by clicking the **Connection Information** button for the namespace, not the event hub itself.  This connection string must have send permissions to send the message to the Event Hub stream.
- `direction` : Must be set to *out*. 

#### Azure Event Hub C# code example for output binding

This example uses a Timer Trigger input, but Event Hubs output can work with any trigger.
 
	using System;
	
	public static void Run(TimerInfo myTimer, out string outputEventHubMessage, TraceWriter log)
	{
	    String msg = $"TimerTriggerCSharp1 executed at: {DateTime.Now}";
	
	    log.Verbose(msg);   
	    
	    outputEventHubMessage = msg;
	}

#### Azure Event Hub JavaScript code example for output binding

This example uses a Timer Trigger input, but Event Hubs output can work with any trigger.
 
	module.exports = function (context, myTimer) {
	    var timeStamp = new Date().toISOString();
	    
	    if(myTimer.isPastDue)
	    {
	        context.log('TimerTriggerJS1 is running late!');
	    }

	    context.log('TimerTriggerJS1 function ran!', timeStamp);   
	    
	    context.bindings.outputEventHubMessage = "TimerTriggerJS1 ran at : " + timeStamp;
	
	    context.done();
	};
