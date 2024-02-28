# Redis PubSub Trigger Binding - C#

The `Redis PubSub Trigger Binding` invokes the function on messages published to a channel.

## How it works

For a `RedisPubSubTrigger` to work, you must provide a Redis channel name that defines the pubsub channel that messages will be read from.
Add a `RedisConnectionString` field to your `local.settings.json` with the connection string:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "RedisConnectionString": "<cacheName>.redis.cache.windows.net:6380,password=<password>"
  }
}
```

## Learn more

For more information, visit the [azure-functions-redis-extension repository](https://github.com/Azure/azure-functions-redis-extension).