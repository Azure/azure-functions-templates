# DaprServiceInvocationTrigger - C<span>#</span>

Using `Dapr Topic Trigger`, your azure functions can react to a message published on a Topic mentioned in your function.

## How it works

`Dapr Topic Trigger` uses Dapr's pub/sub API to receive messages published on a topic.

Publish and subscribe (pub/sub) enables microservices to communicate with each other using messages for event-driven architectures.

The producer, or publisher, writes messages to an input channel and sends them to a topic, unaware which application will receive them.
The consumer, or subscriber, subscribes to the topic and receives messages from an output channel, unaware which service produced these messages.

When using pub/sub in Dapr:

Your service makes a network call to a Dapr pub/sub building block API.
The pub/sub building block makes calls into a Dapr pub/sub component that encapsulates a specific message broker.
To receive messages on a topic, Dapr subscribes to the pub/sub component on behalf of your service with a topic and delivers the messages to an endpoint on your service when they arrive.

For more information, see the official [docs](https://aka.ms/azure-function-dapr-trigger-topic).