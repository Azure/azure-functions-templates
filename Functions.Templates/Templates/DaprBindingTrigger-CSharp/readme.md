# DaprServiceInvocationTrigger - C<span>#</span>

`Dapr Binding Trigger` provides input binding capabilities for applications and a consistent approach to interacting with different cloud/on-premise services or systems.
Developers can have the Dapr runtime trigger an application with input bindings.

## How it works

With `Dapr Binding Trigger`, you can trigger your application when an event from an external resource occurs. An external resource could be a queue, messaging pipeline, cloud-service, filesystem, etc. An optional payload and metadata may be sent with the request.

Input bindings are ideal for event-driven processing, data pipelines, or generally reacting to events and performing further processing. Dapr input bindings allow you to:

Receive events without including specific SDKs or libraries
Replace bindings without changing your code
Focus on business logic and not the event resource implementation

For more information, see the official [docs](https://aka.ms/azure-function-dapr-trigger-binding).