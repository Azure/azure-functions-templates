#### Settings for Redis List Trigger

See [Azure Functions Redis Extension](https://github.com/Azure/azure-functions-redis-extension) for general information about all Redis Bindings.

The settings for a `RedisListTrigger` trigger specifies the following properties:

- `type` : Must be set to *redisListTrigger*.
- `name` : The name of the parameter that the trigger binds to.
- `direction` : Must be set to *in*.
- `connection` : The name of an app setting that contains the Redis connection information. See the [available connection types](https://github.com/Azure/azure-functions-redis-extension?tab=readme-ov-file#connection-types) for more information.
- `key` : Redis key to read from.
- `pollingIntervalInMs` : How often to poll Redis in milliseconds.
- `maxBatchSize` : Number of entries to pop from the Redis list at one time. These are processed in parallel, with each entry from the list triggering its own function invocation. Only supported on Redis 6.2+
- `listDirection` : The direction to pop elements from the list. Must be set to either *left* or *right*.

#### Redis List Trigger C# code example

This C# code monitors the list at key `listKey`. Please refer to our [samples](https://github.com/Azure/azure-functions-redis-extension/tree/main/samples) for more examples.

```csharp
[Function("SimpleRedisListTrigger")]
public void Run(
    [RedisListTrigger("Redis", "listKey")] string entry,
    ILogger logger)
{
    logger.LogInformation(entry);
}
```
