#### Settings for Kafka trigger binding

The settings for an Kafka trigger specifies the following properties:

- `type` : Must be set to *kafkaTrigger*
- `name` : The variable name used in function code for kafka message.
- `direction` : Must be set to *in*.
- `brokerList` : The list of the brokers as a CSV list of broker host or host:port. Or The name of AppSettings for the reference.
- `topic`: The name of the topic.
- `username`: The SASL username for use with the PLAIN and SASL-SCRAM. For EventHubs, It must be `$ConnectionString`.
- `password`: The SASL password for use with the PLAIN and SASL-SCRAM. For EventHubs, It is the `ConnectionString`.
- `protocol`: Security protocol used to communicate with brokers. Choose `NOTSET`, `PLAINTEXT`, `SSL`, `SASLPLAINTEXT`, or `SASLSSL`. `NOTSET` is the default if unspecified. For EventHubs, it must be `SASLSSL`.
- `authenticationMode`: SASL mechanism to use for authentication. Choose `NOTSET`, `GSSAPI`, `PLAIN`, `SCRAMSHA256` or `SCRAMSHA512`. `NOTSET` is the default if unspecified. For EventHubs, it must be `PLAIN`. 
- `consumerGroup`: The name of the consumer group.

The following is the additional configuration. Add these parameters by yourself if it is necessary.

- `SslKeyLocation`: Path to client's private key (PEM) used for authentication.
- `SslKeyPassword`: Password for client's certificate.
- `SslCertificateLocation`: Path to client's certificate.
- `SslCaLocation`: Path to CA certificate file for verifying the broker's certificate.

#### Kafka trigger C# example
 
    #r "Microsoft.Azure.WebJobs.Extensions.Kafka"

    using System;
    using System.Text;
    using Microsoft.Azure.WebJobs.Extensions.Kafka;

    public static void Run(KafkaEventData<string> eventData, ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {eventData.Value}");
    }

#### Kafka trigger JavaScript example

    var string_decode = require('string_decoder').StringDecoder;

    module.exports = async function (context, event) {
        const dec = new string_decode('utf-8');
        let event_str = dec.write(event);

        context.log.info(`JavaScript Kafka trigger function called for message ${event_str}`);
    };
