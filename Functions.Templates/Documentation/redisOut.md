#### Settings for Redis Output Binding

See [Azure Functions Redis Extension](https://github.com/Azure/azure-functions-redis-extension) for general information about all Redis Bindings.

The settings for a `RedisOutput` binding specifies the following properties:

- `type` : Must be set to *redis*.
- `name` : The name of the parameter that the value binds to.
- `direction` : Must be set to *out*.
- `connection` : The name of an app setting that contains the Redis connection information. See the [available connection types](https://github.com/Azure/azure-functions-redis-extension?tab=readme-ov-file#connection-types) for more information.
- `command` : The Redis command to be executed on the cache without any arguments. (e.g. `"GET"`, `"HGET"`)

#### Redis Output Binding C# code example

This C# code deletes any key that was recently set when keyspace notifications are enabled. Please refer to our [samples](https://github.com/Azure/azure-functions-redis-extension/tree/main/samples) for more examples.

```csharp
[FunctionName(nameof(SetDeleter))]
public static void SetDeleter(
    [RedisPubSubTrigger("Redis", "__keyevent@0__:set")] ChannelMessage message,
    [Redis("Redis", "DEL")] out string arguments,
    ILogger logger)
{
    logger.LogInformation($"Deleting recently SET key '{message.Message}'");
    arguments = message.Message;
}
```
