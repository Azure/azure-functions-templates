# KafkaTrigger - JavaScript

The `KafkaTrigger` makes it incredibly easy to react to new events from a Kafka Broker. This sample demonstrates a simple use case of processing data from a given Kafka Broker using JavaScript.

## How it works

For a `KafkaTrigger` to work, you must provide a topic name which dictates where the messages should be read from with authentication.

## Configuration

### EventHubs for Kafka

Add `BrokerList` and `KafkaPassword` to your `local.settings.json`

_local.settings.json_

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "node",
    "BrokerList": "{YOUR_EVENT_HUBS_NAMESPACE}.servicebus.windows.net:9093",
    "KafkaPassword": "{EVENT_HUBS_CONNECTION_STRING}"
  }
}
```

### Others

Modify `function.json` or `KafkaTrigger` attribute according to your broker.