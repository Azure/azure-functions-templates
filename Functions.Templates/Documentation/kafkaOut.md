#### Settings for Kafka output binding

The settings for an Kafka output binding specifies the following properties:

- `type` : Must be set to *kafka*
- `name` : The variable name used in function code for kafka message.
- `direction` : Must be set to *in*.
- `brokerList` : The list of the brokers as a CSV list of broker host or host:port. Or The name of AppSettings for the reference.
- `topic`: The name of the topic.
- `username`: The SASL username for use with the PLAIN and SASL-SCRAM. For EventHubs, It must be `$ConnectionString`.
- `password`: The SASL password for use with the PLAIN and SASL-SCRAM. For EventHubs, It is the `ConnectionString`.
- `protocol`: Security protocol used to communicate with brokers. Choose `NOTSET`, `PLAINTEXT`, `SSL`, `SASLPLAINTEXT`, or `SASLSSL`. `NOTSET` is the default if unspecified. For EventHubs, it must be `SASLSSL`.
- `authenticationMode`: SASL mechanism to use for authentication. Choose `NOTSET`, `GSSAPI`, `PLAIN`, `SCRAMSHA256` or `SCRAMSHA512`. `NOTSET` is the default if unspecified. For EventHubs, it must be `PLAIN`. 

The following is the additional configuration. Add these parameters by yourself if it is necessary.

- `SslKeyLocation`: Path to client's private key (PEM) used for authentication.
- `SslKeyPassword`: Password for client's certificate.
- `SslCertificateLocation`: Path to client's certificate.
- `SslCaLocation`: Path to CA certificate file for verifying the broker's certificate.

#### Kafka C# code example for output binding

This example uses a Timer Trigger input, but Kafka output can work with any trigger.
 
	using System;
	
	public static void Run(TimerInfo myTimer, out string outputKafkaMessage, ILogger log)
	{
	    String msg = $"TimerTriggerCSharp1 executed at: {DateTime.Now}";
	
	    log.LogInformation(msg);   
	    
	    outputKafkaMessage = msg;
	}

#### Kafka JavaScript code example for output binding

This example uses a Http Trigger input, but Kafka output can work with any trigger.
 
	module.exports = async function (context, req) {
		context.log('JavaScript HTTP trigger function processed a request.');

		const message = (req.query.message || (req.body && req.body.message));
		const responseMessage = message
			? "Message received: " + message + ". The message transfered to the kafka broker."
			: "This HTTP triggered function executed successfully. Pass a message in the query string or in the request body for a personalized response.";
		context.bindings.outputKafkaMessage = "Message : " + message;
		context.res = {
			// status: 200, /* Defaults to 200 */
			body: responseMessage
		};
	}
